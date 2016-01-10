namespace TeraTaleNet
{
    public class Sync : RPC
    {
        public string member;
        public Packet packet = new NullPacket();

        public Sync(string receiver, string member)
            : base(RPCType.Specific, receiver)
        {
            this.member = member;
        }

        public Sync(byte[] data)
            : base(data)
        { }
    }
}