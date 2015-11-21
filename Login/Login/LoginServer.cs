using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoboNet;
using TeraTaleNet;

namespace Login
{
    class LoginServer : Server,MessageListener
    {
        Messenger _messenger;
        Task database;
        Task gameServer;
        Task proxy;
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(this);

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
                while (stopped == false)
                    _messenger.Dispatch("Database");
            });

            gameServer = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("GameServer");
            });

            proxy = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Proxy");
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
            database.Wait();
            gameServer.Wait();
            proxy.Wait();
        }

        [RPC]
        void OnLoginRequest(Packet packet)
        {
            _messenger.Send("Database", packet);
        }

        [RPC]
        void OnLoginResponse(Packet packet)
        {
            _messenger.Send("Proxy", packet);
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