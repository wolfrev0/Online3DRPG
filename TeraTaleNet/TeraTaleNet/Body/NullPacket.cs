namespace TeraTaleNet
{
    public class NullPacket : Body
    {
        public NullPacket()
        { }

        public NullPacket(byte[] data)
            : base(data)
        { }
    }
}