using System;
using System.Collections.Generic;
using System.Threading;

namespace TeraTaleNet
{
    public abstract class Server : IServer
    {
        Messenger<string> _messenger = new Messenger<string>();
        bool stopped = false;

        public virtual void Execute()
        {
            _messenger.Start();
            try
            {
                while (stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            _messenger.Join();
        }

        protected void Register(string key, PacketStream stream)
        {
            _messenger.Register(key, stream);
        }

        protected void Send(string key, Packet packet)
        {
            _messenger.Send(key, packet);
        }

        protected abstract void OnUpdate();

        protected void Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegateByPacketType)
        {
            try
            {
                while (stopped == false)
                {
                    while (_messenger.CanReceive(key))
                    {
                        var packet = _messenger.Receive(key);
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

        protected void Stop()
        {
            stopped = true;
        }
    }
}
