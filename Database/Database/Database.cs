using System;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Database
{
    class Database : Server
    {
        DatabaseListener _listener = new DatabaseListener();
        Messenger _messenger;
        Task login;
        Task gameServer;
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(_listener);

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