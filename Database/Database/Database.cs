using System;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Database
{
    class Database : Server
    {
        DatabaseHandler _handler = new DatabaseHandler();
        Messenger _messenger;
        Task _login;
        Task _town;
        Task _forest;
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(_handler);

            Bind("127.0.0.1", Port.DatabaseForLogin, 1);
            _messenger.Register("Login", Listen());
            Console.WriteLine("Login connected.");

            Bind("127.0.0.1", Port.DatabaseForTown, 1);
            _messenger.Register("Town", Listen());
            Console.WriteLine("Town connected.");

            Bind("127.0.0.1", Port.DatabaseForForest, 1);
            _messenger.Register("Forest", Listen());
            Console.WriteLine("Forest connected.");

            _login = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Login");
            });

            _town = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Town");
            });

            _forest = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Forest");
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
            _login.Wait();
            _town.Wait();
            _forest.Wait();
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