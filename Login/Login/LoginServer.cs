using System;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer
    {
        Messenger _proxy;

        public LoginServer()
        {
            _proxy = ListenProxy();
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
            finally
            {
                _proxy.Join();
            }
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
        }

        void OnLoginRequest(LoginRequest request)
        {
            LoginResponse response;
            if(IsValidLogin(request.id, request.pw))
                response = new LoginResponse(true);
            else
                response = new LoginResponse(false);
            _proxy.Send(new Packet(response));
        }

        bool IsValidLogin(string id, string pw)
        {
            return id == "root" && pw == "1234";
        }
    }
}