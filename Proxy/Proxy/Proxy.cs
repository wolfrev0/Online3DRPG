using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeraTaleNet;

namespace Proxy
{
    partial class Proxy : NetworkProgram, MessageHandler, IDisposable
    {
        NetworkAgent _agent = new NetworkAgent();
        Messenger _messenger;
        Messenger _confirmMessenger;
        List<string> _clientKeys = new List<string>();
        Dictionary<string, Task> _dispatchers = new Dictionary<string, Task>();
        Dictionary<string, string> _worldByUser = new Dictionary<string, string>();
        Task _accepter;
        Task _confirm;
        Task _client;
        int _currentConfirmId = 0;
        object _lock = new object();
        HashSet<NetworkInstantiateInfo> _instantiationBuffer = new HashSet<NetworkInstantiateInfo>();
        int _curSignallerID = 1;

        public Task Client
        {
            get { return _client; }
            set { _client = value; }
        }

        protected override void OnStart()
        {
            _messenger = new Messenger(this);
            _confirmMessenger = new Messenger(this);

            Action<Port> connector = (Port port) =>
            {
                var stream = _agent.Connect("127.0.0.1", port);
                stream.Write(new ConnectorInfo("Proxy"));
                _messenger.Register(port.ToString(), stream);
                Console.WriteLine(port.ToString() + " connected.");
                _worldByUser.Add(port.ToString(), port.ToString());
            };

            connector(Port.Login);
            connector(Port.Town);
            connector(Port.Forest);

            foreach (var key in _messenger.Keys)
            {
                var dispatcher = Task.Run(() =>
                {
                    while (stopped == false)
                        _messenger.Dispatch(key);
                });
                _dispatchers.Add(key, dispatcher);
            }

            _accepter = Task.Run(() =>
            {
                _agent.Bind("0.0.0.0", Port.Proxy, 4);
                while (stopped == false)
                {
                    if (_agent.HasConnectReq())
                    {
                        string stringID = _currentConfirmId.ToString();
                        int ID = _currentConfirmId;
                        lock (_lock)
                        {
                            _confirmMessenger.Register(stringID, _agent.Listen());
                            _confirmMessenger.Send(stringID, new ConfirmID(ID));
                        }
                        _currentConfirmId++;
                        Console.WriteLine("Client connected.");
                    }
                }
            });

            _confirm = Task.Run(() =>
            {
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var confirmID in _confirmMessenger.Keys)
                            _confirmMessenger.Dispatch(confirmID);
                    }
                }
            });

            _client = Task.Run(() =>
            {
                while (stopped == false)
                {
                    lock (_lock)
                    {
                        foreach (var key in _clientKeys)
                            _messenger.Dispatch(key);
                    }
                }
            });

            _messenger.Start();
            _confirmMessenger.Start();
        }

        protected override void OnEnd()
        {
            _accepter.Wait();
            foreach (var task in _dispatchers.Values)
                task.Wait();
            _confirm.Wait();
            _client.Wait();
            Dispose();
        }

        protected override void OnUpdate()
        {
            if (Console.KeyAvailable)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    Stop();
            }
        }

        public void Dispose()
        {
            _messenger.Join();
            _confirmMessenger.Join();
            _agent.Dispose();
            GC.SuppressFinalize(this);
        }

        void LoginQuery(Messenger messenger, string key, LoginQuery query)
        {
            _messenger.Send("Login", query);
        }

        void LoginAnswer(Messenger messenger, string key, LoginAnswer answer)
        {
            if (answer.accepted)
            {
                var keys = (ICollection<string>)_clientKeys;
                if (keys.Contains(answer.name))
                {
                    answer.accepted = false;
                    _confirmMessenger.Send(answer.confirmID.ToString(), answer);
                }
                else
                {
                    lock (_lock)
                    {
                        PacketStream stream = _confirmMessenger.Unregister(answer.confirmID.ToString());
                        _messenger.Register(answer.name, stream);
                        _clientKeys.Add(answer.name);
                    }
                    _worldByUser.Add(answer.name, answer.world);
                    _messenger.Send(answer.world, new PlayerJoin(answer.name));
                    _messenger.Send(answer.name, answer);
                    foreach (var instantiationInfo in _instantiationBuffer)
                        _messenger.Send(answer.name, instantiationInfo);
                }
            }
            else
            {
                _confirmMessenger.Send(answer.confirmID.ToString(), answer);
            }
        }

        void NetworkInstantiateRequest(Messenger messenger, string key, NetworkInstantiateRequest req)
        {
            _instantiationBuffer.Add(new NetworkInstantiateInfo(req.owner, req.prefabIndex, _curSignallerID));
            _messenger.Send(_worldByUser[req.owner], new NetworkInstantiateInfo(req.owner, req.prefabIndex, _curSignallerID));
            foreach(var user in _clientKeys)
            {
                if (_worldByUser[user] == _worldByUser[req.owner])
                    _messenger.Send(user, new NetworkInstantiateInfo(req.owner, req.prefabIndex, _curSignallerID));
            }
            _curSignallerID++;
        }

        void MessageHandler.RPCHandler(RPC rpc)
        {
            if ((rpc.rpcType & RPCType.Self) == RPCType.Self)
            {
                _messenger.Send(rpc.sender, rpc);
            }
            if ((rpc.rpcType & RPCType.Server) == RPCType.Server)
            {
                _messenger.Send(_worldByUser[rpc.sender], rpc);
            }
            if ((rpc.rpcType & RPCType.Others) == RPCType.Others)
            {
                foreach(var target in _clientKeys)
                {
                    if (target != rpc.sender)
                        _messenger.Send(target, rpc);
                }
            }
            if ((rpc.rpcType & RPCType.Buffered) == RPCType.Buffered)
            { }
        }
    }
}