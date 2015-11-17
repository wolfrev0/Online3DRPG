using System;
using System.Threading;
using LoboNet;
using TeraTaleNet;

namespace Proxy
{
    class ProxyServer
    {
        Messenger _messenger = new Messenger();

        public ProxyServer()
        {
            _messenger.Register("Login", ConnectToLogin());
            _messenger.Register("Client", ListenClient());
        }

        PacketStream ConnectToLogin()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Proxy);
            Console.WriteLine("Login Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ListenClient()
        {
            var _listener = new TcpListener("0.0.0.0", (ushort)TargetPort.Client, 4);
            var connection = _listener.Accept();
            Console.WriteLine("Client Connected.");
            _listener.Dispose();

            return new PacketStream(connection);
        }

        public void Execute()
        {
            _messenger.Start();
            try
            {
                MainLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            _messenger.Join();
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

                if (_messenger.CanReceive("Login"))
                {
                    var packet = _messenger.Receive("Login");
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }

                if (_messenger.CanReceive("Client"))
                {
                    var packet = _messenger.Receive("Client");
                    switch (packet.header.type)
                    {
                        case PacketType.LoginRequest:
                            OnLoginRequest((LoginRequest)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }
                Thread.Sleep(10);
            }
        }

        void OnLoginResponse(LoginResponse response)
        {
            _messenger.Send("Client", new Packet(response));
        }

        void OnLoginRequest(LoginRequest request)
        {
            _messenger.Send("Login", new Packet(request));
        }
    }
}