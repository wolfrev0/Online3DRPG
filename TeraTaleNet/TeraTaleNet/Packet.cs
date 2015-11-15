using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public class Packet
    {
        Header _header;
        IBody _body;

        public Header header { get { return _header; } }
        public IBody body { get { return _body; } }

        static public Packet Create(Header header, byte[] bytes)
        {
            IBody body;
            switch (header.type)
            {
                case PacketType.WriteConsoleRequest:
                    body = new WriteConsoleRequest(bytes);
                    break;
                case PacketType.LoginRequest:
                    body = new LoginRequest(bytes);
                    break;
                case PacketType.LoginResponse:
                    body = new LoginResponse(bytes);
                    break;
                default:
                    throw new ArgumentException("PacketType does not matched.");
            }
            return new Packet(header, body);
        }

        public Packet(Header header, IBody body)
        {
            _header = header;
            _body = body;
        }

        public Packet(IBody body)
        {
            _header = body.CreateHeader();
            _body = body;
        }
    }
}
