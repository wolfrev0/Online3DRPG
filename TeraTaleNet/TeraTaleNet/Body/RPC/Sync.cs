namespace TeraTaleNet
{
    public class Sync : RPC
    {
        public string member;
        public Packet packet = new NullPacket();

        public Sync(RPCType type, string receiver, string member)
            : base(type, receiver)
        {
            this.member = member;
        }

        public Sync(byte[] data)
            : base(data)
        { }
    }
}