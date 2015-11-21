using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer : Server, IDisposable
    {
        Messenger _messenger = new Messenger();
        Messenger _clientMessenger = new Messenger();
        Messenger _confirmMessenger = new Messenger();
        Task _accepter;
        Task _login;
        Task _gameServer;
        Task _confirm;
        Task _client;
        int _currentConfirmId = 0;
        object _lock = new object();
        bool _disposed = false;

        public Task Client
        {
            get { return _client; }
            set { _client = value; }
        }

        protected override void OnStart()
        {
            _messenger.Register("Login", Connect("127.0.0.1", Port.LoginForProxy));
            Console.WriteLine("Login connected.");

            _messenger.Register("GameServer", Connect("127.0.0.1", Port.GameServer));
            Console.WriteLine("GameServer connected.");

            _accepter = Task.Run(() =>
            {
                Bind("0.0.0.0", Port.Proxy, 4);
                while (stopped == false)
                {
                    if (HasConnectReq())
                    {
                        string stringID = _currentConfirmId.ToString();
                        int ID = _currentConfirmId;
                        lock (_lock)
                        {
                            _confirmMessenger.Register(stringID, Listen());
                            _confirmMessenger.Send(stringID, new Packet(new ConfirmID(ID)));
                        }
                        _currentConfirmId++;
                        Console.WriteLine("Client connected.");
                    }
                }
            });

            _login = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                while (stopped == false)
                    _messenger.Dispatch("Login", delegates);
            });

            _gameServer = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                while (stopped == false)
                    _messenger.Dispatch("GameServer", delegates);
            });

            _confirm = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginRequest, OnLoginRequest);
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var key in _confirmMessenger.Keys)
                            _confirmMessenger.Dispatch(key, delegates);
                    }
                }
            });

            _client = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var confirmID in _clientMessenger.Keys)
                            _clientMessenger.Dispatch(confirmID, delegates);
                    }
                }
            });

            _messenger.Start();
            _clientMessenger.Start();
            _confirmMessenger.Start();
        }

        protected override void OnEnd()
        {
            _accepter.Wait();
            _login.Wait();
            _gameServer.Wait();
            _confirm.Wait();
            _client.Wait();
        }

        protected override void OnUpdate()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    Stop();
            }
        }

        void OnLoginResponse(Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;

            if (response.accepted)
            {
                lock (_lock)
                {
                    PacketStream stream = _confirmMessenger.Unregister(response.confirmID.ToString());
                    _clientMessenger.Register(response.nickName, stream);
                }
                _clientMessenger.Send(response.nickName, packet);
            }
            else
            {
                _confirmMessenger.Send(response.confirmID.ToString(), new Packet(response));
            }
        }

        void OnLoginRequest(Packet packet)
        {
            _messenger.Send("Login", packet);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        _messenger.Join();
                        _clientMessenger.Join();
                        _confirmMessenger.Join();
                    }
                    _disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}