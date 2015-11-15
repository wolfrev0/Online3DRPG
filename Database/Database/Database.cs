using System;
using LoboNet;
using TeraTaleNet;

namespace Database
{
    class Database
    {
        Messenger _login;

        public Database()
        {
            _login = ListenLogin();
        }

        Messenger ListenLogin()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)TargetPort.Login, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Login Connected.");
            _listener.Dispose();

            return new Messenger(new PacketStream(connection));
        }

        public void Execute()
        {
            _login.Start();
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
            if (IsValidLogin(request.id, request.pw))
                response = new LoginResponse(true);
            else
                response = new LoginResponse(false);
            _login.Send(new Packet(response));
        }

        bool IsValidLogin(string id, string pw)
        {
            return id == "root" && pw == "1234";
        }
    }
}