using System;
using System.Threading;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class Messenger<T>
    {
        //concurrent Dictionary?
        ConcurrentDictionary<T, ConcurrentQueue<Packet>> _sendQByKey = new ConcurrentDictionary<T, ConcurrentQueue<Packet>>();
        ConcurrentDictionary<T, ConcurrentQueue<Packet>> _recvQByKey = new ConcurrentDictionary<T, ConcurrentQueue<Packet>>();
        ConcurrentDictionary<T, PacketStream> _streamByKey = new ConcurrentDictionary<T, PacketStream>();
        Thread _sender;
        Thread _receiver;
        bool _stopped = false;

        public Dictionary<T, PacketStream>.KeyCollection Keys
        {
            get
            {
                return _streamByKey.Keys;
            }
        }

        public Messenger()
        {
            _sender = new Thread(Sender);
            _receiver = new Thread(Receiver);
        }

        public void Start()
        {
            _sender.Start();
            _receiver.Start();
        }

        public void Register(T key, PacketStream stream)
        {
            _streamByKey.Add(key, stream);
            _sendQByKey.Add(key, new ConcurrentQueue<Packet>());
            _recvQByKey.Add(key, new ConcurrentQueue<Packet>());
        }

        public PacketStream Unregister(T key)
        {
            var ret = _streamByKey[key];
            _streamByKey.Remove(key);
            return ret;
        }
        
        public void Join()
        {
            _stopped = true;
  
            foreach(var stream in _streamByKey.Values)
            {
                stream.Dispose();
            }
            _sender.Join();
            _receiver.Join();
        }

        public void Send(T key, Packet packet)
        {
            _sendQByKey[key].Enqueue(packet);
        }

        public Packet Receive(T key)
        {
            return _recvQByKey[key].Dequeue();
        }

        public bool CanReceive(T key)
        {
            return _recvQByKey[key].Count > 0;
        }

        void Sender()
        {
            try
            {
                while (_stopped == false)
                {
                    foreach(var key in Keys)
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
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
            }
        }
    }
}