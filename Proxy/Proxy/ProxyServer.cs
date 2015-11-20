using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer : Server
    {
        Messenger _messenger = new Messenger();
        Messenger _clientMessenger = new Messenger();
        Messenger _confirmMessenger = new Messenger();
        int currentConfirmId = 0;
        Task accepter;
        Task login;
        Task gameServer;
        bool stopped = false;

        protected override void OnStart()
        {
            _messenger.Register("Login", Connect("127.0.0.1", Port.LoginForProxy));
            Console.WriteLine("Login connected.");

            _messenger.Register("GameServer", Connect("127.0.0.1", Port.GameServer));
            Console.WriteLine("GameServer connected.");

            accepter = Task.Run(() =>
            {
                Bind("0.0.0.0", Port.Proxy, 4);
                while (stopped == false)
                {
                    if (HasConnectReq())
                    {
                        _confirmMessenger.Register(currentConfirmId++.ToString(), Listen());
                        Console.WriteLine("Client connected.");
                    }
                }
            });

            login = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                _messenger.Dispatcher("Login", delegates);
            });

            gameServer = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                _messenger.Dispatcher("GameServer", delegates);
            });

            _messenger.Start();
            _clientMessenger.Start();
            _confirmMessenger.Start();
        }

        protected override void OnEnd()
        {
            stopped = true;
            accepter.Wait();
            _messenger.Join();
            _clientMessenger.Join();
            _confirmMessenger.Join();
            login.Wait();
            gameServer.Wait();
        }

        protected override void OnUpdate()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    Stop();
            }

            foreach (var key in _clientMessenger.Keys)
            {
                while (_clientMessenger.CanReceive(key))
                {
                    var packet = _clientMessenger.Receive(key);
                    switch (packet.header.type)
                    {
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }
            }

            foreach (var confirmID in _confirmMessenger.Keys)
            {
                while (_confirmMessenger.CanReceive(confirmID))
                {
                    var packet = _confirmMessenger.Receive(confirmID);
                    switch (packet.header.type)
                    {
                        case PacketType.LoginRequest:
                            OnLoginRequest((LoginRequest)packet.body, int.Parse(confirmID));
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }
            }
        }

        void OnLoginResponse(Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;

            if (response.accepted)
            {
                PacketStream stream = _confirmMessenger.Unregister(response.confirmID.ToString());
                _clientMessenger.Register(response.nickName, stream);
                _clientMessenger.Send(response.nickName, new Packet(response));
            }
            else
            {
                _confirmMessenger.Send(response.confirmID.ToString(), new Packet(response));
            }
        }

        void OnLoginRequest(LoginRequest request, int confirmID)
        {
            request.confirmID = confirmID;
            _messenger.Send("Login", new Packet(request));
        }
    }
}