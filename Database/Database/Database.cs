using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Database
{
    class Database : NetworkProgram, IDisposable
    {
        DatabaseHandler _handler = new DatabaseHandler();
        NetworkAgent _agent = new NetworkAgent();
        Messenger _messenger;
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();

        protected override void OnStart()
        {
            History.Log(Packet.GetIndexByType(typeof(Equip)).ToString());
            var e = new Equip(new WeaponNull());
            e.sender = "";
            e.signallerID = 0;
            var p = new Packet(e);
            var s = p.Serialize();

            History.Log(Packet.GetTypeByIndex(11).Name);

            _messenger = new Messenger(_handler);

            Action listenner = () =>
            {
                _agent.Bind("127.0.0.1", Port.Database, 1);
                var stream = _agent.Listen();
                var info = (ConnectorInfo)stream.Read().body;
                _messenger.Register(info.name, stream);
                Console.WriteLine(info.name + " connected.");
            };
            
            listenner();//Login
            listenner();//Town
            listenner();//Forest

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
            foreach(var task in _dispatchers.Values)
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