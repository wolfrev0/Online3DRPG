using System;
using System.Net;
using System.Net.Sockets;

namespace LoboNet
{
    public class TcpClient : IDisposable
    {
        bool disposed = false;

        public TcpClient()
        {
        }

        public Connection Connect(string ip, ushort port)
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.NoDelay = true;
            server.SendBufferSize = 0;
            server.Connect(IPAddress.Parse(ip), port);
            return new Connection(server);
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
            disposed = true;
        }
    }
}
