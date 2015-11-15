using System;
using LoboNet;

namespace TeraTaleNet
{
    public class PacketStream : IDisposable
    {
        const int _bufferSize = 1024;
        bool disposed = false;

        Connection _connection;

        public PacketStream(Connection connection)
        {
            _connection = connection;
        }

        public Packet Read()
        {
            byte[] buffer = new byte[_bufferSize];

            _connection.Read(buffer, Header.size);
            Header header = new Header(buffer);
            _connection.Read(buffer, header.bodySize);
            return Packet.Create(header, buffer);
        }

        public void Write(Packet packet)
        {
            byte[] bytes = packet.header.Serialize();
            _connection.Write(bytes, bytes.Length);
            bytes = packet.body.Serialize();
            _connection.Write(bytes, bytes.Length);
        }

        public bool HasPacket()
        {
            return _connection.PollRead();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            _connection.Dispose();
            disposed = true;
        }
    }
}
