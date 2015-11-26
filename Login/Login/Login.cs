using System;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Login
{
    class Login : Server
    {
        LoginHandler _handler = new LoginHandler();
        Messenger _messenger;
        Task _database;
        Task _proxy;
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(_handler);

            _messenger.Register("Database", Connect("127.0.0.1", Port.DatabaseForLogin));
            Console.WriteLine("Database connected.");

            Bind("127.0.0.1", Port.LoginForProxy, 1);
            _messenger.Register("Proxy", Listen());
            Console.WriteLine("Proxy connected.");

            _database = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Database");
            });

            _proxy = Task.Run(() =>
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
            _database.Wait();
            _proxy.Wait();
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