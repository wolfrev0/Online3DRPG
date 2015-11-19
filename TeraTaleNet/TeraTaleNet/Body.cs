using System;

namespace TeraTaleNet
{
    public abstract class Body : PacketData
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
    }
}