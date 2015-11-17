using System;
using System.Threading;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class Messenger
    {
        //concurrent Dictionary?
        ConcurrentDictionary<string, ConcurrentQueue<Packet>> _sendQs = new ConcurrentDictionary<string, ConcurrentQueue<Packet>>();
        ConcurrentDictionary<string, ConcurrentQueue<Packet>> _recvQs = new ConcurrentDictionary<string, ConcurrentQueue<Packet>>();
        ConcurrentDictionary<string, PacketStream> _streams = new ConcurrentDictionary<string, PacketStream>();
        Thread _sender;
        Thread _receiver;
        bool _stopped = false;

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

        public void Register(string key, PacketStream stream)
        {
            _streams.Add(key, stream);
            _sendQs.Add(key, new ConcurrentQueue<Packet>());
            _recvQs.Add(key, new ConcurrentQueue<Packet>());
        }
        
        public void Join()
        {
            _stopped = true;
  
            foreach(var stream in _streams.Values)
            {
                stream.Dispose();
            }
            _sender.Join();
            _receiver.Join();
        }

        public void Send(string key, Packet packet)
        {
            _sendQs[key].Enqueue(packet);
        }

        public Packet Receive(string key)
        {
            return _recvQs[key].Dequeue();
        }

        public bool CanReceive(string key)
        {
            return _recvQs[key].Count > 0;
        }

        void Sender()
        {
            try
            {
                while (_stopped == false)
                {
                    foreach(var key in _sendQs.Keys)
                    {
                        if (_sendQs[key].Count > 0)
                        {
                            //Need ioLock?
                            _streams[key].Write(_sendQs[key].Dequeue());
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
                    foreach (var key in _recvQs.Keys)
                    {
                        if (_streams[key].HasPacket())
                        {
                            //Need ioLock?
                            _recvQs[key].Enqueue(_streams[key].Read());
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