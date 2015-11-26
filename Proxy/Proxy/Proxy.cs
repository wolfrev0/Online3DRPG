using System;
using System.Collections.Generic;
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
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();
        Task _accepter;
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



            PacketStream stream;
            ConnectorInfo info;

            stream = Connect("127.0.0.1", Port.Login);
            stream.Write(new ConnectorInfo("Proxy"));
            _messenger.Register("Login", stream);
            Console.WriteLine("Login connected.");

            stream = Connect("127.0.0.1", Port.Town);
            stream.Write(new ConnectorInfo("Proxy"));
            _messenger.Register("Town", stream);
            Console.WriteLine("Town connected.");

            stream = Connect("127.0.0.1", Port.Forest);
            stream.Write(new ConnectorInfo("Proxy"));
            _messenger.Register("Forest", stream);
            Console.WriteLine("Forest connected.");

            //Bind("127.0.0.1", Port.Login, 1);
            //stream = Listen();
            //info = (ConnectorInfo)stream.Read().body;
            //_messenger.Register(info.name, stream);
            //Console.WriteLine(info.name + " connected.");

            Task dispatcher;

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Login");
            });
            _dispatchers.Add("Login", dispatcher);

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Town");
            });
            _dispatchers.Add("Town", dispatcher);

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Forest");
            });
            _dispatchers.Add("Forest", dispatcher);

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
            foreach (var task in _dispatchers.Values)
                task.Wait();
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