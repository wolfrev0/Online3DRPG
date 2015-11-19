using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

namespace Database
{
    class Database : Server
    {
        static string accountLocation = "Accounts\\";
        static string playerInfoLocation = "PlayerInfo\\";
        Messenger _messenger = new Messenger();

        public Database()
        {
            _messenger.Register("Login", ListenLogin());
            _messenger.Register("GameServer", ListenGameServer());

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginRequest, OnLoginRequest);
                _messenger.Dispatcher("Login", delegates);
            });

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.PlayerInfoRequest, OnPlayerInfoRequest);
                _messenger.Dispatcher("GameServer", delegates);
            });
        }

        PacketStream ListenLogin()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)Port.DatabaseForLogin, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Login Connected.");
            _listener.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ListenGameServer()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)Port.DatabaseForGameServer, 1);
            var connection = _listener.Accept();
            Console.WriteLine("GameServer Connected.");
            _listener.Dispose();

            return new PacketStream(connection);
        }

        protected override void OnUpdate()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    _messenger.Join();
                    Stop();
                }
            }
        }

        void OnLoginRequest(Packet packet)
        {
            LoginRequest request = (LoginRequest)packet.body;
            LoginResponse response;
            try
            {
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
            catch (IOException)
            {
                response = new LoginResponse(false, RejectedReason.InvalidID, "Login", request.confirmID);
            }
            _messenger.Send("Login", new Packet(response));
        }

        void OnPlayerInfoRequest(Packet packet)
        {
            PlayerInfoRequest request = (PlayerInfoRequest)packet.body;
            using (var stream = new StreamReader(new FileStream(playerInfoLocation + request.nickName, FileMode.Open)))
            {
                string world = stream.ReadLine();

                PlayerInfoResponse response = new PlayerInfoResponse(request.nickName, world);
                _messenger.Send("GameServer", new Packet(response));
            }
        }
    }
}