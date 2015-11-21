using System;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public abstract class Server : IServer, IDisposable
    {
        TcpListener _listener;
        bool _stopped = false;
        bool _disposed = false;

        public bool stopped { get { return _stopped; } }

        public void Bind(string ip, Port port, int backlog)
        {
            if (_listener != null)
                _listener.Dispose();
            _listener = new TcpListener(ip, (ushort)port, backlog);
        }

        public void Execute()
        {
            try
            {
                OnStart();
                while (_stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                History.Log(e.ToString());
            }
            finally
            {
                History.Save();
                OnEnd();
            }
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        protected bool HasConnectReq()
        {
            return _listener.HasConnectReq();
        }

        protected PacketStream Listen()
        {
            return new PacketStream(_listener.Accept());
        }

        protected PacketStream Connect(string ip, Port port)
        {
            var _connecter = new TcpConnecter();
            return new PacketStream(_connecter.Connect(ip, (ushort)port));
        }

        public void Stop()
        {
            _stopped = true;
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

        ~Server()
        {
            Dispose(false);
        }
    }
}
