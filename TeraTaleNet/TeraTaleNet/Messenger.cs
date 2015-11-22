using System;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TeraTaleNet
{
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

        Dictionary<string, MethodInfo> rpcByName = new Dictionary<string, MethodInfo>();

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
                if (method.GetCustomAttributes(typeof(RPC), false).Length > 0)
                {
                    rpcByName.Add(method.Name, method);
                }
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
            _streamByKey.Add(key, stream);
            _sendQByKey.Add(key, new ConcurrentQueue<Packet>());
            _recvQByKey.Add(key, new ConcurrentQueue<Packet>());
        }

        public PacketStream Unregister(string key)
        {
            var ret = _streamByKey[key];
            _streamByKey.Remove(key);
            return ret;
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
                rpcByName[packet.header.type.ToString()].Invoke(listener, new object[] { this, key, packet });
            }
            Thread.Sleep(10);
        }

        public void DispatcherCoroutine(string key)
        {
            while (CanReceive(key))
            {
                var packet = Receive(key);
                rpcByName[packet.header.type.ToString()].Invoke(listener, new object[] { this, key, packet });
            }
        }

        void Sender()
        {
            try
            {
                while (_stopped == false)
                {
                    foreach (var key in Keys)
                    {
                        if (_sendQByKey[key].Count > 0)
                        {
                            //Need ioLock?
                            _streamByKey[key].Write(_sendQByKey[key].Dequeue());
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                History.Log(e.ToString());
            }
            finally
            {
                History.Save();
            }
        }

        void Receiver()
        {
            try
            {
                while (_stopped == false)
                {
                    foreach (var key in Keys)
                    {
                        if (_streamByKey[key].HasPacket())
                        {
                            //Need ioLock?
                            _recvQByKey[key].Enqueue(_streamByKey[key].Read());
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                History.Log(e.ToString());
            }
            finally
            {
                History.Save();
            }
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
                    foreach (var stream in _streamByKey.Values)
                        stream.Dispose();
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