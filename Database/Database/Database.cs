using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeraTaleNet;

namespace Database
{
    class Database : Server
    {
        static string accountLocation = "Accounts\\";
        static string playerInfoLocation = "PlayerInfo\\";
        Messenger _messenger = new Messenger();

        protected override void OnStart()
        {
            _messenger.Register("Login", Listen("127.0.0.1", Port.DatabaseForLogin, 1));
            Console.WriteLine("Login connected.");
            _messenger.Register("GameServer", Listen("127.0.0.1", Port.DatabaseForGameServer, 1));
            Console.WriteLine("GameServer connected.");

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

            _messenger.Start();
        }

        protected override void OnUpdate()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    Stop();
            }
        }

        protected override void OnEnd()
        {
            _messenger.Join();
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