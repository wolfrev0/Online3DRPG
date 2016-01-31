using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Login
{
    class Login : NetworkProgram, IDisposable
    {
        LoginHandler _handler = new LoginHandler();
        NetworkAgent _agent = new NetworkAgent();
        Messenger _messenger;
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();

        protected override void OnStart()
        {
            _messenger = new Messenger(_handler);

            PacketStream stream;
            ConnectorInfo info;

            stream = _agent.Connect("127.0.0.1", Port.Database);
            stream.Write(new ConnectorInfo("Login"));
            _messenger.Register("Database", stream);
            Console.WriteLine("Database connected.");

            //Listen Proxy
            _agent.Bind("127.0.0.1", Port.Login, 1);
            stream = _agent.Listen();
            info = (ConnectorInfo)stream.Read().body;
            _messenger.Register(info.name, stream);
            Console.WriteLine(info.name + " connected.");

            foreach (var key in _messenger.Keys)
            {
                var dispatcher = Task.Run(() =>
                {
                    while (stopped == false)
                        _messenger.Dispatch(key);
                });
                _dispatchers.Add(key, dispatcher);
            }

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
            Dispose();
        }

        public void Dispose()
        {
            _messenger.Join();
            _agent.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}