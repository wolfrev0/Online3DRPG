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
        HashSet<string> _loggedInUsers = new HashSet<string>();

        PacketStream ConnectToDatabase()
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect("127.0.0.1", (ushort)Port.DatabaseForLogin);
            Console.WriteLine("Database Connected.");
            _connecter.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ListenGameServer()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)Port.LoginForGameServer, 1);
            var connection = _listener.Accept();
            Console.WriteLine("GameServer Connected.");
            _listener.Dispose();

            return new PacketStream(connection);
        }

        PacketStream ListenProxy()
        {
            var _listener = new TcpListener("127.0.0.1", (ushort)Port.LoginForProxy, 1);
            var connection = _listener.Accept();
            Console.WriteLine("Proxy Connected.");
            _listener.Dispose();

            return new PacketStream(connection);
        }

        protected override void OnStart()
        {
            _messenger.Register("Database", ConnectToDatabase());
            _messenger.Register("GameServer", ListenGameServer());
            _messenger.Register("Proxy", ListenProxy());

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                _messenger.Dispatcher("Database", delegates);
            });

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                _messenger.Dispatcher("GameServer", delegates);
            });

            Task.Run(() =>
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