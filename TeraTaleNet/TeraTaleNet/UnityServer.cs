using LoboNet;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class UnityServer : MonoBehaviour, IServer
    {
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
        }

        protected void Stop()
        {
            Destroy(gameObject);
        }

        protected PacketStream Listen(string ip, Port port, int backlog)
        {
            var _listener = new TcpListener(ip, (ushort)port, backlog);
            var connection = _listener.Accept();
            _listener.Dispose();

            return new PacketStream(connection);
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
