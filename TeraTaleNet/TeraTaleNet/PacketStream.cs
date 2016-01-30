using System;
using LoboNet;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class PacketStream : IDisposable
    {
        const int _bufferSize = 1024;

        Connection _connection;
        bool _disposed = false;

        public PacketStream(Connection connection)
        {
            _connection = connection;
        }

        public Packet Read()
        {
            byte[] buffer = new byte[_bufferSize];
            
            _connection.Read(buffer, Header.size);
            var header = new Header(buffer);
            _connection.Read(buffer, header.bodySize);
            return Packet.Create(header, buffer);
        }

        public void Write(Packet packet)
        {
            byte[] bytes = packet.Serialize();
            _connection.Write(bytes, bytes.Length);
        }

        public bool HasPacket()
        {
            return _connection.CanRead();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }

            }
            _disposed = true;
        }

        ~PacketStream()
        {
            Dispose(false);
        }
    }
}
