using System;
using System.Net;
using System.Net.Sockets;

namespace LoboNet
{
    public class TcpConnecter
    {
        public Connection Connect(string ip, ushort port)
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.NoDelay = true;
            server.SendBufferSize = 0;
            server.Connect(IPAddress.Parse(ip), port);
            return new Connection(server);
        }
    }
}