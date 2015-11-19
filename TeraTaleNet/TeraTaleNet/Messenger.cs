using System;
using System.Threading;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class Messenger
    {
        //concurrent Dictionary?
        Dictionary<string, ConcurrentQueue<Packet>> _sendQByKey = new Dictionary<string, ConcurrentQueue<Packet>>();
        Dictionary<string, ConcurrentQueue<Packet>> _recvQByKey = new Dictionary<string, ConcurrentQueue<Packet>>();
        Dictionary<string, PacketStream> _streamByKey = new Dictionary<string, PacketStream>();
        Thread _sender;
        Thread _receiver;
        bool _stopped = false;

        public Dictionary<string, PacketStream>.KeyCollection Keys
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

        public void Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegateByPacketType)
        {
            try
            {
                while (_stopped == false)
                {
                    while (CanReceive(key))
                    {
                        var packet = Receive(key);
                        delegateByPacketType[packet.header.type](packet);
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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

        public void Join()
        {
            _stopped = true;

            foreach (var stream in _streamByKey.Values)
            {
                stream.Dispose();
            }
            _sender.Join();
            _receiver.Join();
        }

        public void Send(string key, Packet packet)
        {
            _sendQByKey[key].Enqueue(packet);
        }

        public Packet Receive(string key)
        {
            return _recvQByKey[key].Dequeue();
        }

        public Packet ReceiveSync(string key)
        {
            while (CanReceive(key) == false)
            { }
            return _recvQByKey[key].Dequeue();
        }

        public bool CanReceive(string key)
        {
            return _recvQByKey[key].Count > 0;
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
                            var packet = _sendQByKey[key].Dequeue();
                            _streamByKey[key].Write(packet);
                            Console.WriteLine(packet.header.type);
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
                            var packet = _streamByKey[key].Read();
                            _recvQByKey[key].Enqueue(packet);
                            Console.WriteLine(packet.header.type);
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