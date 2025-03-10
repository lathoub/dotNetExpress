using System.Net;

namespace dotNetExpress.Tools;

public class Net
{
    public static bool IsLocalIpAddress(string host)
    {
        try
        { // get host IP addresses
            var hostIPs = Dns.GetHostAddresses(host);
            // get local IP addresses
            var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            // test if any host IP equals to any local IP or to localhost
            foreach (var hostIp in hostIPs)
            {
                // is localhost
                if (IPAddress.IsLoopback(hostIp)) return true;
                // is local address
                if (localIPs.Contains(hostIp))
                    return true;
            }
        }
        catch
        {
            // ignored
        }

        return false;
    }
}
