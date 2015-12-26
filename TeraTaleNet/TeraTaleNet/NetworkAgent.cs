using System;
using LoboNet;

namespace TeraTaleNet
{
    public class NetworkAgent : IDisposable
    {
        TcpListener _listener;
        bool _disposed = false;

        public void Bind(string ip, Port port, int backlog)
        {
            if (_listener != null)
                _listener.Dispose();
            _listener = new TcpListener(ip, (ushort)port, backlog);
        }

        public bool HasConnectReq()
        {
            return _listener.HasConnectReq();
        }

        public PacketStream Listen()
        {
            return new PacketStream(_listener.Accept());
        }

        public PacketStream Connect(string ip, Port port)
        {
            var _connecter = new TcpConnecter();
            return new PacketStream(_connecter.Connect(ip, (ushort)port));
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
                    if (_listener != null)
                        _listener.Dispose();
                }

            }
            _disposed = true;
        }

        ~NetworkAgent()
        {
            Dispose(false);
        }
    }
}
