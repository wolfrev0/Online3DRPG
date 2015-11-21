using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeraTaleNet;

namespace Database
{
    class Database : Server, MessageListener
    {
        static string accountLocation = "Accounts\\";
        static string playerInfoLocation = "PlayerInfo\\";
        Messenger _messenger;
        Task login;
        Task gameServer;
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(this);

            Bind("127.0.0.1", Port.DatabaseForLogin, 1);
            _messenger.Register("Login", Listen());
            Console.WriteLine("Login connected.");

            Bind("127.0.0.1", Port.DatabaseForGameServer, 1);
            _messenger.Register("GameServer", Listen());
            Console.WriteLine("GameServer connected.");

            login = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Login");
            });

            gameServer = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("GameServer");
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
            login.Wait();
            gameServer.Wait();
        }

        [RPC]
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

        [RPC]
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

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        _messenger.Join();
                    }
                    _disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}