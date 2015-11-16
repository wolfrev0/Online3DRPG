using System;
using System.Collections.Generic;
using System.Threading;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer
    {
        Messenger _database;
        Messenger _proxy;
        //Dictionary<string, Messenger>

        public LoginServer()
        {
            _database = ConnectToDatabase();
            _proxy = ListenProxy();
        }

        Messenger ConnectToDatabase()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Login);
            Console.WriteLine("Database Connected.");
            _connecter.Dispose();

            return new Messenger(new PacketStream(connection));
        }

        Messenger ListenProxy()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)TargetPort.Proxy, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Proxy Connected.");
            _listener.Dispose();

            return new Messenger(new PacketStream(connection));
        }

        public void Execute()
        {
            _database.Start();
            _proxy.Start();
            try
            {
                MainLoop();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            _database.Join();
            _proxy.Join();
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

                if (_database.CanReceive())
                {
                    var packet = _database.Receive();
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }

                if (_proxy.CanReceive())
                {
                    var packet = _proxy.Receive();
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
            _database.Send(new Packet(request));
        }

        void OnLoginResponse(LoginResponse response)
        {
            if(response.accepted)
            {

            }
            _proxy.Send(new Packet(response));
        }
    }
}