using System;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class ProxyServer
    {
        PacketStream _stream;
        Messenger _messenger;

        public ProxyServer()
        {
            //var _accepter = new Accepter("", 256, 4);
            var _client = new TcpClient();
            var connection = _client.Connect("127.0.0.1", 9852);
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
            catch (Exception e)
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
            _messenger.Send(new Packet(new LoginRequest("root", "1234")));
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        break;
                }

                if (_messenger.CanReceive())
                {
                    var packet = _messenger.Receive();
                    switch (packet.header.type)
                    {
                        case PacketType.LoginResponse:
                            OnLoginResponse((LoginResponse)packet.body);
                            break;
                        default:
                            throw new ArgumentException("Received invalid packet type.");
                    }
                }
            }
        }

        void OnLoginResponse(LoginResponse request)
        {
            if(request.accepted)
            {
                Console.WriteLine("Accepted.");
            }
            else
            {
                Console.WriteLine("Rejected.");
            }
        }
    }
}