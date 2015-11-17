using System;
using System.Collections.Generic;
using System.Threading;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer
    {
        Messenger _messenger= new Messenger();
        HashSet<string> loggedInUsers = new HashSet<string>();

        public LoginServer()
        {
            _messenger.Register("Database", ConnectToDatabase());
            _messenger.Register("Proxy", ListenProxy());
        }

        PacketStream ConnectToDatabase()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Login);
            Console.WriteLine("Database Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ListenProxy()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)TargetPort.Proxy, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Proxy Connected.");
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
            catch(Exception e)
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
                if(Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        break;
                }

                if (_messenger.CanReceive("Database"))
                {
                    var packet = _messenger.Receive("Database");
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }

                if (_messenger.CanReceive("Proxy"))
                {
                    var packet = _messenger.Receive("Proxy");
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
            Thread.Sleep(10);
        }

        void OnLoginRequest(LoginRequest request)
        {
            _messenger.Send("Database", new Packet(request));
        }

        void OnLoginResponse(LoginResponse response)
        {
            if (response.accepted && loggedInUsers.Contains(response.nickName))
            {
                response.accepted = false;
                response.reason = RejectedReason.LoggedInAlready;
            }
            _messenger.Send("Proxy", new Packet(response));
        }
    }
}