using System;

namespace TeraTaleNet
{
    public class Header : ISerializable
    {
        static public int size = sizeof(PacketType) + sizeof(int);

        PacketType _type;
        int _bodySize;

        public PacketType type { get { return _type; } }
        public int bodySize { get { return _bodySize; } }

        public Header(PacketType type, int bodySize)
        {
            _type = type;
            _bodySize = bodySize;
        }

        public Header(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public byte[] Serialize()
        {
            var typeBytes = BitConverter.GetBytes((int)type);
            var bodySizeBytes = BitConverter.GetBytes(_bodySize);

            var ret = new byte[typeBytes.Length + bodySizeBytes.Length];

            int offset = 0;
            typeBytes.CopyTo(ret, offset);
            offset += typeBytes.Length;
            bodySizeBytes.CopyTo(ret, offset);

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _type = (PacketType)BitConverter.ToInt32(buffer, offset);
            offset += sizeof(PacketType);
            _bodySize = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
        }
    }
}
