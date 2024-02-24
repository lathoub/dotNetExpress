﻿using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using dotNetExpress.Overrides;

namespace dotNetExpress;

internal class Utils
{
    internal class Parameters
    {
        public Express Express { get; }
        public TcpClient TcpClient { get; }
        private Parameters(Express express, TcpClient tcpClient)
        {
            Express = express;
            TcpClient = tcpClient;
        }

        public static Parameters CreateInstance(Express express, TcpClient tcpClient)
        {
            return new Parameters(express, tcpClient);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string HashKey(string key)
    {
        const string handshakeKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        var longKey = key + handshakeKey;
        var hashBytes = SHA1.HashData(Encoding.ASCII.GetBytes(longKey));

        return Convert.ToBase64String(hashBytes);
    }
    
    /// <summary>
    /// 
    /// </summary>
    private static readonly List<TcpClient> _webSockets = new();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static bool IsWebSocketUpgradeRequest(Request req)
    {
        return (string.Equals(req.Get("connection"), "Upgrade", StringComparison.OrdinalIgnoreCase)
                && string.Equals(req.Get("upgrade"), "websocket", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    private static void DoWebSocketUpgradeRequest(Request req, Response res)
    {
        var key = req.Get("sec-websocket-key");

        res.Set("Upgrade", "WebSocket");
        res.Set("Connection", "Upgrade");
        res.Set("Sec-WebSocket-Accept", HashKey(key));

        res._send();

        //lock (_webSockets)
        //{
        //    _webSockets.Add(tcpClient);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateInfo"></param>
    internal static void ClientThread(object stateInfo)
    {
        try
        {
            if (stateInfo is not Parameters param) return;

            var tcpClient = param.TcpClient;
            var stream = tcpClient.GetStream();
            var express = param.Express;

            // Read the http headers
            // Note: the body part is NOT read at this stage
            var headerLines = new List<string>();
            var streamReader = new MessageBodyStreamReader(stream);
            {
                while (true)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    headerLines.Add(line);
                }
            }

            // Construct a Request object, based on the header info
            if (!Request.TryParse(express, headerLines.ToArray(), out var req))
                throw new HttpProtocolException(500, "Unable to construct Request", new ProtocolViolationException("Unable to construct Request"));

            // get the IP from the remove endPoint
            if (tcpClient.Client.RemoteEndPoint is IPEndPoint ipEndPoint)
                req.Ip = ipEndPoint.Address;

            // Make Response object
            req.Res = new Response(express, stream);

            if (!string.IsNullOrEmpty(req.Get("content-length")))
            {
                var contentLength = int.Parse(req.Get("content-length"));

                // When a content-length is available, a stream is provided in Request
                req.StreamReader = streamReader;
                req.StreamReader.SetLength(contentLength);
            }

            if (IsWebSocketUpgradeRequest(req))
            {
                DoWebSocketUpgradeRequest(req, req.Res);

                // These socket are not disposed, but kept!
                lock (_webSockets)
                {
                    _webSockets.Add(tcpClient);
                }
            }
            else
            {
                req.Res.App.Router().Dispatch(req, req.Res);
                req.Res._send();
            }
        }
        catch (HttpProtocolException e)
        {

        }
        catch (Exception e)
        {

        }
    }
}
