using System;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public class Accepter
    {
        TcpListener _listener;
        Thread _accepter;
        bool _stopped = false;

        public delegate void AcceptCallback(PacketStream connection);
        public AcceptCallback onAccepted;

        public Accepter(string ip, ushort port, int backlogSize)
        {
            _listener = new TcpListener(ip, port, backlogSize);
            _accepter = new Thread(Worker);
        }

        public void Start()
        {
            _accepter.Start();
        }

        public void Join()
        {
            _stopped = true;
            _accepter.Join();
        }

        void Worker()
        {
            try
            {
                while (_stopped == false)
                {
                    if (_listener.HasConnectReq())
                    {
                        var connection = _listener.Accept();
                        onAccepted(new PacketStream(connection));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}