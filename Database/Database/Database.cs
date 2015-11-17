using System;
using System.Threading;
using System.IO;
using LoboNet;
using TeraTaleNet;

namespace Database
{
    class Database
    {
        static string accountLocation = "Accounts\\";
        Messenger<string> _messenger = new Messenger<string>();

        public Database()
        {
            _messenger.Register("Login", ListenLogin());
        }

        PacketStream ListenLogin()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)TargetPort.Login, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Login Connected.");
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
            LoginResponse response;
            try {
                using (var stream = new StreamReader(new FileStream(accountLocation + request.id, FileMode.Open)))
                {
                    string pw = stream.ReadLine();
                    string nickName = stream.ReadLine();
                    if (request.pw == pw)
                    {
                        response = new LoginResponse(true, RejectedReason.Accepted, nickName, request.confirmID);
                    }
                    else
                    {
                        response = new LoginResponse(false, RejectedReason.InvalidPW, "Login", request.confirmID);
                    }
                }
            }
            catch(IOException)
            {
                response = new LoginResponse(false, RejectedReason.InvalidID, "Login", request.confirmID);
            }
            _messenger.Send("Login", new Packet(response));
        }
    }
}