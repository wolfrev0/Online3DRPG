using System;
using System.Net;
using System.Net.Sockets;

namespace LoboNet
{
    public class TcpListener : IDisposable
    {
        Socket _listener;
        bool disposed = false;

        public TcpListener(string ip, ushort port, int backlogSize)
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _listener.Listen(backlogSize);
        }
        
        public Connection Accept()
        {
            return new Connection(_listener.Accept());
        }

        public bool HasConnectReq()
        {
            return _listener.Poll(0, SelectMode.SelectRead);
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

            _listener.Close();
            disposed = true;
        }
    }
}