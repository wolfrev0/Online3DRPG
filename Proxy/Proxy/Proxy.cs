using System;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Proxy
{
    partial class Proxy : Server
    {
        ProxyHandler _handler;
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
            _handler = new ProxyHandler(this);
            _messenger = new Messenger(_handler);
            _clientMessenger = new Messenger(_handler);
            _confirmMessenger = new Messenger(_handler);

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