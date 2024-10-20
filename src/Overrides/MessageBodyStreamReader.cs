﻿using System.IO;
using System.Text;

namespace dotNetExpress.Overrides;

public class MessageBodyStreamReader(Stream inner) : Stream
{
    private long _length;

    public string FileName;

    public override bool CanRead => inner.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => _length;

    public override long Position { get; set; }

    public override void Flush() => inner.Flush();

    public override long Seek(long offset, SeekOrigin origin) { return 0; }

    public override void SetLength(long value) => _length = value;

    public override int Read(byte[] buffer, int offset, int bytesToRead)
    {
        if (Position + bytesToRead > Length)
            bytesToRead = (int)(Length - Position);

        if (bytesToRead == 0)
            return 0;

        var bytesRead = inner.Read(buffer, offset, bytesToRead);

        Position += bytesRead;

        return bytesRead;
    }

    public string ReadLine()
    {
        var buffer = new byte[8192];
        var i = 0;
        for (; i < buffer.Length; i++)
        {
            buffer[i] = (byte)inner.ReadByte();
            if (buffer[i] == '\n')
                break;
        }

        if (i == buffer.Length) return string.Empty;

        if (buffer[i - 1] == '\r')
            i--;

        return Encoding.UTF8.GetString(buffer, 0, i);
    }

    public override void Write(byte[] buffer, int offset, int count) { }
}