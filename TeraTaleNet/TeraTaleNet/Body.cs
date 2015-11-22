using System;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public abstract class Body : Serializable
    {
        protected Body()
        { }

        protected Body(byte[] data)
            : base(data)
        { }

        public Header CreateHeader()
        {
            var type = (PacketType)Enum.Parse(typeof(PacketType), GetType().Name);
            return new Header(type, SerializedSize());
        }

        public static implicit operator Packet(Body body)
        {
            return new Packet(body);
        }
    }
}