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
            Header header = new Header(0, 0);

            bool successed = false;
            do
            {
                _connection.Read(buffer, Header.size);
                string ss="";
                for (int i = 0; i < Header.size; i++)
                    ss += buffer[i] + ",";
                History.Log(ss);
                header = new Header(buffer);
                try
                {
                    Packet.GetTypeByIndex(header.type);
                    History.Log("Header " + Packet.GetTypeByIndex(header.type).Name);
                    successed = true;
                }
                catch (KeyNotFoundException)
                {
                    History.Log("Invalid header detected.");
                }
            } while (successed == false);

            _connection.Read(buffer, header.bodySize);
            string sss = "";
            for (int i = 0; i < header.bodySize; i++)
                sss += buffer[i] + ",";
            History.Log(sss);
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
