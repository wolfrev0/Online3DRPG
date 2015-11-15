using System;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public class Accepter
    {
        ConcurrentQueue<Connection> _connnectionQ = new ConcurrentQueue<Connection>();
        TcpServer _server;
        Thread _accepter;
        bool _stopped = false;

        public delegate void AcceptCallback(Connection connection);
        public AcceptCallback onAccepted;

        public Accepter(string ip, ushort port, int backlogSize)
        {
            _server = new TcpServer(ip, port, backlogSize);
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
                    if (_server.HasConnectReq())
                    {
                        var connection = _server.Accept();
                        _connnectionQ.Enqueue(connection);
                        onAccepted(connection);
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