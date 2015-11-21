using System;
using LoboNet;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class UnityServer : MonoBehaviour, IServer, IDisposable
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

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }

        void OnDestroy()
        {
            OnEnd();
            Dispose();
        }

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

        protected void Stop()
        {
            _stopped = true;
            Destroy(gameObject);
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

        ~UnityServer()
        {
            Dispose(false);
        }
    }
}
