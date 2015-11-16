using System;
using System.Threading;

namespace TeraTaleNet
{
    public class Messenger
    {
        ConcurrentQueue<Packet> _sendQ = new ConcurrentQueue<Packet>();
        ConcurrentQueue<Packet> _recvQ = new ConcurrentQueue<Packet>();
        PacketStream _stream;
        Thread _sender;
        Thread _receiver;
        bool _stopped = false;

        public Messenger(PacketStream stream)
        {
            _stream = stream;
            _sender = new Thread(Sender);
            _receiver = new Thread(Receiver);
        }

        public void Start()
        {
            _sender.Start();
            _receiver.Start();
        }

        public void Join()
        {
            _stopped = true;
            _sender.Join();
            _receiver.Join();
            _stream.Dispose();
        }

        public void Send(Packet packet)
        {
            _sendQ.Enqueue(packet);
        }

        public Packet Receive()
        {
            return _recvQ.Dequeue();
        }

        public bool CanReceive()
        {
            return _recvQ.Count > 0;
        }

        void Sender()
        {
            try
            {
                while (_stopped == false)
                {
                    if (_sendQ.Count > 0)
                    {
                        //Need ioLock?
                        _stream.Write(_sendQ.Dequeue());
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
                    if (_stream.HasPacket())
                    {
                        //Need ioLock?
                        _recvQ.Enqueue(_stream.Read());
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