using System;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class ProxyServer
    {
        Messenger _login;
        Messenger _client;

        public ProxyServer()
        {
            _login = ConnectToLogin();
            _client = ListenClient();
        }

        Messenger ConnectToLogin()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Proxy);
            Console.WriteLine("Login Connected.");
            _connecter.Dispose();

            return new Messenger(new PacketStream(connection));
        }

        Messenger ListenClient()
        {
            var _listener = new TcpListener("0.0.0.0", (ushort)TargetPort.Client, 4);
            var connection = _listener.Accept();
            Console.WriteLine("Client Connected.");
            _listener.Dispose();

            return new Messenger(new PacketStream(connection));
        }

        public void Execute()
        {
            _login.Start();
            _client.Start();
            try
            {
                MainLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                _login.Join();
                _client.Join();
            }
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

                if (_login.CanReceive())
                {
                    var packet = _login.Receive();
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }

                if (_client.CanReceive())
                {
                    var packet = _client.Receive();
                    switch (packet.header.type)
                    {
                        case PacketType.LoginRequest:
                            OnLoginRequest((LoginRequest)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }
            }
        }

        void OnLoginResponse(LoginResponse response)
        {
            _client.Send(new Packet(response));
        }

        void OnLoginRequest(LoginRequest request)
        {
            _login.Send(new Packet(request));
        }
    }
}