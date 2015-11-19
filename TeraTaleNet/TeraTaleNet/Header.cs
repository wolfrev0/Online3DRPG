namespace TeraTaleNet
{
    public class Header : PacketData
    {
        static public int size = sizeof(PacketType) + sizeof(int);

        public PacketType type;
        public int bodySize;

        public Header(PacketType type, int bodySize)
        {
            this.type = type;
            this.bodySize = bodySize;
        }

        public Header(byte[] data)
            : base(data)
        { }
    }
}