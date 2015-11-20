using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer : Server
    {
        Messenger _messenger = new Messenger();
        Task database;
        Task gameServer;
        Task proxy;
        HashSet<string> _loggedInUsers = new HashSet<string>();

        protected override void OnStart()
        {
            _messenger.Register("Database", Connect("127.0.0.1", Port.DatabaseForLogin));
            Console.WriteLine("Database connected.");

            Bind("127.0.0.1", Port.LoginForGameServer, 1);
            _messenger.Register("GameServer", Listen());
            Console.WriteLine("GameServer connected.");

            Bind("127.0.0.1", Port.LoginForProxy, 1);
            _messenger.Register("Proxy", Listen());
            Console.WriteLine("Proxy connected.");

            database = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                _messenger.Dispatcher("Database", delegates);
            });

            gameServer = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                _messenger.Dispatcher("GameServer", delegates);
            });

            proxy = Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginRequest, OnLoginRequest);
                _messenger.Dispatcher("Proxy", delegates);
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
            database.Wait();
            gameServer.Wait();
            proxy.Wait();
        }

        void OnLoginRequest(Packet packet)
        {
            _messenger.Send("Database", packet);
        }

        void OnLoginResponse(Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;
            if (response.accepted)
            {
                if (_loggedInUsers.Contains(response.nickName))
                {
                    response.accepted = false;
                    response.reason = RejectedReason.LoggedInAlready;
                }
                else
                {
                    _loggedInUsers.Add(response.nickName);
                    _messenger.Send("GameServer", new Packet(new PlayerLogin(response.nickName)));
                }
            }
            _messenger.Send("Proxy", new Packet(response));
        }
    }
}