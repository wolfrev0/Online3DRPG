using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Database
{
    class Database : Server
    {
        DatabaseHandler _handler = new DatabaseHandler();
        Messenger _messenger;
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();
        bool _disposed = false;

        protected override void OnStart()
        {
            _messenger = new Messenger(_handler);

            PacketStream stream;
            ConnectorInfo info;

            Bind("127.0.0.1", Port.Database, 1);
            stream = Listen();
            info = (ConnectorInfo)stream.Read().body;
            _messenger.Register(info.name, stream);
            Console.WriteLine(info.name + " connected.");

            Bind("127.0.0.1", Port.Database, 1);
            stream = Listen();
            info = (ConnectorInfo)stream.Read().body;
            _messenger.Register(info.name, stream);
            Console.WriteLine(info.name + " connected.");

            Bind("127.0.0.1", Port.Database, 1);
            stream = Listen();
            info = (ConnectorInfo)stream.Read().body;
            _messenger.Register(info.name, stream);
            Console.WriteLine(info.name + " connected.");

            Task dispatcher;

            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Login");
            });
            _dispatchers.Add("Login", dispatcher);
            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Town");
            });
            _dispatchers.Add("Town", dispatcher);
            dispatcher = Task.Run(() =>
            {
                while (stopped == false)
                    _messenger.Dispatch("Forest");
            });
            _dispatchers.Add("Forest", dispatcher);

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
            foreach(var task in _dispatchers.Values)
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