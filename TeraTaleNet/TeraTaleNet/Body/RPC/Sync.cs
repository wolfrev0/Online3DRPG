namespace TeraTaleNet
{
    public class Sync : RPC
    {
        public string member;
        public IAutoSerializable data = new NullPacket();

        public Sync(RPCType type, string receiver, string member)
            : base(type, receiver)
        {
            this.member = member;
        }

        public Sync()
        { }
    }
}