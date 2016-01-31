using System;
using System.Reflection;

namespace TeraTaleNet
{
    public class Packet
    {
        public Header header;
        public Body body;

        static public Packet Create(Header header, byte[] bytes)
        {
            var type = Type.GetType("TeraTaleNet." + Body.GetNameByIndex(header.type));
            Body body = (Body)Activator.CreateInstance(type, bytes);
            return new Packet(header, body);
        }

        Packet(Header header, Body body)
        {
            this.header = header;
            this.body = body;
        }

        public Packet(Body body)
        {
            header = body.CreateHeader();
            this.body = body;
        }

        public byte[] Serialize()
        {
            var headerBytes = header.Serialize();
            var bodyBytes = body.Serialize();

            int offset = 0;
            byte[] ret = new byte[headerBytes.Length + bodyBytes.Length];

            headerBytes.CopyTo(ret, offset);
            offset += headerBytes.Length;
            bodyBytes.CopyTo(ret, offset);
            offset += bodyBytes.Length;

            return ret;
        }

        public int SerializedSize()
        {
            return header.SerializedSize() + body.SerializedSize();
        }
    }
}
