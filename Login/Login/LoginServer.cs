﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer : Server
    {
        HashSet<string> loggedInUsers = new HashSet<string>();

        public LoginServer()
        {
            Register("Database", ConnectToDatabase());
            Register("GameServer", ListenGameServer());
            Register("Proxy", ListenProxy());

            Task.Run(() =>
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginResponse, OnLoginResponse);
                Loop("Database", delegates);
            });

            Task.Run(() => 
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                Loop("GameServer", delegates);
            });

            Task.Run(() => 
            {
                var delegates = new Dictionary<PacketType, PacketDelegate>();
                delegates.Add(PacketType.LoginRequest, OnLoginRequest);
                Loop("Proxy", delegates);
            });
        }

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

        protected override void MainLoop()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    Stop();
            }
        }

        void OnLoginRequest(Packet packet)
        {
            Send("Database", packet);
        }

        void OnLoginResponse(Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;
            if (response.accepted)
            {
                if (loggedInUsers.Contains(response.nickName))
                {
                    response.accepted = false;
                    response.reason = RejectedReason.LoggedInAlready;
                }
                else
                {
                    loggedInUsers.Add(response.nickName);
                    Send("GameServer", new Packet(new PlayerLogin(response.nickName)));
                }
            }
            Send("Proxy", new Packet(response));
        }
    }
}