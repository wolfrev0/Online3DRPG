using System;
using System.Threading;
using LoboNet;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer
    {
        Accepter _accepter = new Accepter("0.0.0.0", (ushort)Port.Proxy, 4);
        Messenger<string> _serverMessenger = new Messenger<string>();
        Messenger<string> _clientMessenger = new Messenger<string>();
        Messenger<int> _confirmMessenger = new Messenger<int>();
        int currentConfirmId = 0;

        public ProxyServer()
        {
            _serverMessenger.Register("Login", ConnectToLogin());
            _accepter.onAccepted = (PacketStream stream) => 
            {
                _confirmMessenger.Register(currentConfirmId++, stream);
                Console.WriteLine("Client Accepted.");
            };
        }

        PacketStream ConnectToLogin()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)Port.LoginForProxy);
            Console.WriteLine("Login Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        public void Execute()
        {
            _accepter.Start();
            _serverMessenger.Start();
            _clientMessenger.Start();
            _confirmMessenger.Start();
            try
            {
                MainLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            _accepter.Join();
            _serverMessenger.Join();
            _clientMessenger.Join();
            _confirmMessenger.Join();
        }

        void MainLoop()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        break;
                }

                if (_serverMessenger.CanReceive("Login"))
                {
                    var packet = _serverMessenger.Receive("Login");
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }

                foreach (var key in _clientMessenger.Keys)
                {
                    if (_clientMessenger.CanReceive(key))
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
                    if (_confirmMessenger.CanReceive(confirmID))
                    {
                        var packet = _confirmMessenger.Receive(confirmID);
                        switch (packet.header.type)
                        {
                            case PacketType.LoginRequest:
                                OnLoginRequest(confirmID, (LoginRequest)packet.body);
                                break;
                            default:
                                throw new ArgumentException("Received invalid packet type.");
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        void OnLoginResponse(LoginResponse response)
        {
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

        void OnLoginRequest(int confirmID, LoginRequest request)
        {
            request.confirmID = confirmID;
            _serverMessenger.Send("Login", new Packet(request));
        }
    }
}