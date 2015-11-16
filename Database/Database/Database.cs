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
            Thread.Sleep(10);
        }

        void OnLoginRequest(LoginRequest request)
        {
            try {
                using (var stream = new StreamReader(new FileStream(accountLocation + request.id, FileMode.Open)))
                {
                    string pw = stream.ReadLine();
                    string nickName = stream.ReadLine();
                    if (request.pw == pw)
                    {
                        var response = new LoginResponse(true, nickName);
                        _login.Send(new Packet(response));
                    }
                    else
                    {
                        throw new IOException();
                    }
                }
            }
            catch(IOException)
            {
                var response = new LoginResponse(false, "");
                _login.Send(new Packet(response));
            }
        }
    }
}