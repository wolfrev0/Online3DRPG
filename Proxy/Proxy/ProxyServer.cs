using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer : Server
    {
        Accepter _accepter = new Accepter("0.0.0.0", (ushort)Port.Proxy, 4);
        Messenger _messenger = new Messenger();
        Messenger _clientMessenger = new Messenger();
        Messenger _confirmMessenger = new Messenger();
        int currentConfirmId = 0;

        public ProxyServer()
        {
            _messenger.Register("Login", ConnectToLogin());
            _messenger.Register("GameServer", ConnectToGameServer());
            _accepter.onAccepted = (PacketStream stream) => 
            {
                _confirmMessenger.Register(currentConfirmId++.ToString(), stream);
                Console.WriteLine("Client Accepted.");
            };

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                _messenger.Dispatcher("Login", delegates);
            });

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                _messenger.Dispatcher("GameServer", delegates);
            });
        }

        PacketStream ConnectToLogin()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)Port.LoginForProxy);
            Console.WriteLine("Login Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ConnectToGameServer()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)Port.GameServer);
            Console.WriteLine("GameServer Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        protected override void OnStart()
        {
            _accepter.Start();
            _messenger.Start();
            _clientMessenger.Start();
            _confirmMessenger.Start();
        }

        protected override void OnEnd()
        {
            _accepter.Join();
            _messenger.Join();
            _clientMessenger.Join();
            _confirmMessenger.Join();
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