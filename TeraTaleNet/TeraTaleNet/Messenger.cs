using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class DisconnectException : Exception
    {
        public string key;

        public DisconnectException(string key)
        {
            this.key = key;
        }
    }

    public class Messenger : IDisposable
    {
        //concurrent Dictionary?
        Dictionary<string, ConcurrentQueue<Packet>> _sendQByKey = new Dictionary<string, ConcurrentQueue<Packet>>();
        Dictionary<string, ConcurrentQueue<Packet>> _recvQByKey = new Dictionary<string, ConcurrentQueue<Packet>>();
        Dictionary<string, PacketStream> _streamByKey = new Dictionary<string, PacketStream>();
        Thread _sender;
        Thread _receiver;
        MessageHandler listener;
        bool _stopped = false;
        bool _disposed = false;
        object _locker = new object();
        public delegate void OnDisconnected(string name);
        public OnDisconnected onDisconnected = key => { };

        Dictionary<Type, MethodInfo> handlerByType = new Dictionary<Type, MethodInfo>();

        public Dictionary<string, PacketStream>.KeyCollection Keys
        {
            get
            {
                return _streamByKey.Keys;
            }
        }

        public Messenger(MessageHandler listener)
        {
            this.listener = listener;

            foreach (var method in listener.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                try
                {
                    var p = method.GetParameters();
                    if (p.Length > 2)
                        handlerByType.Add(p[2].ParameterType, method);
                    else if (p.Length > 0)
                        handlerByType.Add(p[0].ParameterType, method);
                }
                catch (ArgumentException)
                { }
            }

            _sender = new Thread(Sender);
            _receiver = new Thread(Receiver);
        }

        public void Start()
        {
            _sender.Start();
            _receiver.Start();
        }

        public void Register(string key, PacketStream stream)
        {
            lock(_locker)
                _streamByKey.Add(key, stream);
            _sendQByKey.Add(key, new ConcurrentQueue<Packet>());
            _recvQByKey.Add(key, new ConcurrentQueue<Packet>());
        }

        public PacketStream Unregister(string key)
        {
            lock(_locker)
            {
                var ret = _streamByKey[key];
                _streamByKey.Remove(key);
                return ret;
            }
        }

        public void Send(string key, Packet packet)
        {
            _sendQByKey[key].Enqueue(packet);
        }

        public Packet Receive(string key)
        {
            return _recvQByKey[key].Dequeue();
        }

        public bool CanReceive(string key)
        {
            return _recvQByKey[key].Count > 0;
        }

        public Packet ReceiveSync(string key)
        {
            while (CanReceive(key) == false)
            { }
            return _recvQByKey[key].Dequeue();
        }

        public void Dispatch(string key)
        {
            while (CanReceive(key))
            {
                var packet = Receive(key);
                var rpc = packet.body as RPC;
                if (rpc != null)
                {
                    MethodInfo method;
                    if (handlerByType.TryGetValue(Packet.GetTypeByIndex(packet.header.type), out method))
                        method.Invoke(listener, new object[] { packet.body });
                    listener.RPCHandler(rpc);
                }
                else
                    handlerByType[Packet.GetTypeByIndex(packet.header.type)].Invoke(listener, new object[] { this, key, packet.body });
            }
        }

        public void DispatcherCoroutine(string key)
        {
            var packet = Receive(key);
            var rpc = packet.body as RPC;
            if (rpc != null)
                listener.RPCHandler(rpc);
            else
                handlerByType[Packet.GetTypeByIndex(packet.header.type)].Invoke(listener, new object[] { this, key, packet.body });
        }

        void Sender()
        {
            while (_stopped == false)
            {
                lock (_locker)
                {
                    try
                    {
                        foreach (var key in Keys)
                        {
                            try
                            {
                                if (_sendQByKey[key].Count > 0)
                                {
                                    //Need ioLock?
                                    var packet = _sendQByKey[key].Dequeue();
                                    History.Log("Sended : " + Packet.GetTypeByIndex(packet.header.type));
                                    _streamByKey[key].Write(packet);
                                }
                            }
                            catch (Exception e)
                            {
                                History.Log(e.ToString());
                                throw new DisconnectException(key);
                            }
                        }
                    }
                    catch (DisconnectException e)
                    {
                        _streamByKey[e.key].Dispose();
                        _streamByKey.Remove(e.key);
                        _recvQByKey.Remove(e.key);
                        _sendQByKey.Remove(e.key);
                        onDisconnected(e.key);
                    }
                    finally
                    {
                    }
                }
            }
            History.Save();
        }

        void Receiver()
        {
            while (_stopped == false)
            {
                lock (_locker)
                {
                    try
                    {
                        foreach (var key in Keys)
                        {
                            try
                            {
                                if (_streamByKey[key].HasPacket())
                                {
                                    //Need ioLock?
                                    var packet = _streamByKey[key].Read();
                                    History.Log("Recieved : " + Packet.GetTypeByIndex(packet.header.type));
                                    _recvQByKey[key].Enqueue(packet);
                                }
                            }
                            catch (Exception e)
                            {
                                History.Log(e.ToString());
                                throw new DisconnectException(key);
                            }
                        }

                    }
                    catch (DisconnectException e)
                    {
                        _streamByKey[e.key].Dispose();
                        _streamByKey.Remove(e.key);
                        _sendQByKey.Remove(e.key);
                        _recvQByKey.Remove(e.key);
                        onDisconnected(e.key);
                    }
                    finally
                    {
                    }
                }
            }
            History.Save();
        }

        public void Join()
        {
            _stopped = true;
            _sender.Join();
            _receiver.Join();
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    lock (_locker)
                    {
                        foreach (var stream in _streamByKey.Values)
                            stream.Dispose();
                    }
                }

            }
            _disposed = true;
        }

        ~Messenger()
        {
            Dispose(false);
        }
    }
}