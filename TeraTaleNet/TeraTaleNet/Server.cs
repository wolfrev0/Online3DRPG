using System;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public abstract class Server : IServer
    {
        TcpListener _listener;
        bool _stopped = false;

        public void Bind(string ip, Port port, int backlog)
        {
            if (_listener != null)
                _listener.Dispose();
            _listener = new TcpListener(ip, (ushort)port, backlog);
        }

        public void Execute()
        {
            OnStart();
            try
            {
                while (_stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            OnEnd();
            if (_listener != null)
                _listener.Dispose();
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        protected void Stop()
        {
            _stopped = true;
        }

        protected bool HasConnectReq()
        {
            if (_listener != null)
                return _listener.HasConnectReq();
            return false;
        }

        protected PacketStream Listen()
        {
            if (_listener != null)
                return new PacketStream(_listener.Accept());
            return null;
        }

        protected PacketStream Connect(string ip, Port port)
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect(ip, (ushort)port);
            _connecter.Dispose();

            return new PacketStream(connection);
        }
    }
}
