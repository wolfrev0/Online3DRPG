using System;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer
    {
        PacketStream _stream;
        Messenger _messenger;

        public LoginServer()
        {
            var _server = new TcpServer("127.0.0.1", 9852, 4);
            var connection = _server.Accept();
            Console.WriteLine("Connected.");
            _stream = new PacketStream(connection);
            _messenger = new Messenger(_stream);
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
            finally
            {
                _stream.Dispose();
                _messenger.Join();
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

                if (_messenger.CanReceive())
                {
                    var packet = _messenger.Receive();
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
            _messenger.Send(new Packet(response));
        }

        bool IsValidLogin(string id, string pw)
        {
            return id == "root" && pw == "1234";
        }
    }
}