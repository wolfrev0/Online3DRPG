using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Login
{
    class Login : Server
    {
        LoginHandler _handler = new LoginHandler();
        Messenger _messenger;
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(_handler);

            PacketStream stream;
            ConnectorInfo info;

            stream = Connect("127.0.0.1", Port.Database);
            stream.Write(new ConnectorInfo("Login"));
            _messenger.Register("Database", stream);
            Console.WriteLine("Database connected.");

            Bind("127.0.0.1", Port.Login, 1);
            stream = Listen();
            info = (ConnectorInfo)stream.Read().body;
            _messenger.Register(info.name, stream);
            Console.WriteLine(info.name + " connected.");

            Task dispatcher;

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Database");
            });
            _dispatchers.Add("Database", dispatcher);

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Proxy");
            });
            _dispatchers.Add("Proxy", dispatcher);

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
            foreach (var task in _dispatchers.Values)
                task.Wait();
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