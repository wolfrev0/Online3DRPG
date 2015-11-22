using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer : Server,MessageListener
    {
        Messenger _messenger;
        Messenger _clientMessenger;
        Messenger _confirmMessenger;
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
            _messenger = new Messenger(this);
            _clientMessenger = new Messenger(this);
            _confirmMessenger = new Messenger(this);

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
                            _confirmMessenger.Send(stringID, new ConfirmID(ID));
                        }
                        _currentConfirmId++;
                        Console.WriteLine("Client connected.");
                    }
                }
            });

            _login = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Login");
            });

            _gameServer = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("GameServer");
            });

            _confirm = Task.Run(() =>
            {
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var key in _confirmMessenger.Keys)
                            _confirmMessenger.Dispatch(key);
                    }
                }
            });

            _client = Task.Run(() =>
            {
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var confirmID in _clientMessenger.Keys)
                            _clientMessenger.Dispatch(confirmID);
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

        [RPC]
        void OnLoginResponse(Messenger messenger, string key, Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;
            if (response.accepted)
            {
                var keys = (ICollection<string>)_clientMessenger.Keys;
                if (keys.Contains(response.nickName))
                {
                    response.accepted = false;
                    response.reason = RejectedReason.LoggedInAlready;

                    _confirmMessenger.Send(response.confirmID.ToString(), response);
                }
                else
                {
                    lock (_lock)
                    {
                        PacketStream stream = _confirmMessenger.Unregister(response.confirmID.ToString());
                        _clientMessenger.Register(response.nickName, stream);
                    }
                    _messenger.Send("GameServer", new PlayerLogin(response.nickName));

                    _clientMessenger.Send(response.nickName, response);
                }
            }
        }

        [RPC]
        void OnLoginRequest(Messenger messenger, string key, Packet packet)
        {
            _messenger.Send("Login", packet);
        }

        [RPC]
        void OnPlayerJoin(Messenger messenger, string key, Packet packet)
        {
            PlayerJoin join = (PlayerJoin)packet.body;
            History.Log(join.nickName);
            _clientMessenger.Send(join.nickName, packet);
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