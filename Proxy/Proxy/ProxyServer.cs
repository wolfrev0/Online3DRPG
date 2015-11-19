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
        Messenger<string> _clientMessenger = new Messenger<string>();
        Messenger<int> _confirmMessenger = new Messenger<int>();
        int currentConfirmId = 0;

        public ProxyServer()
        {
            Register("Login", ConnectToLogin());
            Register("GameServer", ConnectToGameServer());
            _accepter.onAccepted = (PacketStream stream) => 
            {
                _confirmMessenger.Register(currentConfirmId++, stream);
                Console.WriteLine("Client Accepted.");
            };

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                Loop("Login", delegates);
            });

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                Loop("GameServer", delegates);
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

        public override void Execute()
        {
            _accepter.Start();
            _clientMessenger.Start();
            _confirmMessenger.Start();

            base.Execute();

            _accepter.Join();
            _clientMessenger.Join();
            _confirmMessenger.Join();
        }

        protected override void MainLoop()
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
                            OnLoginRequest((LoginRequest)packet.body, confirmID);
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
                PacketStream stream = _confirmMessenger.Unregister(response.confirmID);
                _clientMessenger.Register(response.nickName, stream);
                _clientMessenger.Send(response.nickName, new Packet(response));
            }
            else
            {
                _confirmMessenger.Send(response.confirmID, new Packet(response));
            }
        }

        void OnLoginRequest(LoginRequest request, int confirmID)
        {
            request.confirmID = confirmID;
            Send("Login", new Packet(request));
        }
    }
}