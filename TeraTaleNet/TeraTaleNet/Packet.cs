using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public class Packet
    {
        Header _header;
        Body _body;

        public Header header { get { return _header; } }
        public Body body { get { return _body; } }

        static public Packet Create(Header header, byte[] bytes)
        {
            Body body;
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
                case PacketType.PlayerLogin:
                    body = new PlayerLogin(bytes);
                    break;
                case PacketType.PlayerInfoRequest:
                    body = new PlayerInfoRequest(bytes);
                    break;
                case PacketType.PlayerInfoResponse:
                    body = new PlayerInfoResponse(bytes);
                    break;
                default:
                    throw new ArgumentException("PacketType does not matched.");
            }
            return new Packet(header, body);
        }

        public Packet(Header header, Body body)
        {
            _header = header;
            _body = body;
        }

        public Packet(Body body)
        {
            _header = body.CreateHeader();
            _body = body;
        }
    }
}
