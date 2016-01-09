namespace TeraTaleNet
{
    public class Sync : RPC
    {
        public string field;
        public Packet packet = new NullPacket();

        public Sync(string receiver, string field)
            : base(RPCType.Specific, receiver)
        {
            this.field = field;
        }

        public Sync(byte[] data)
            : base(data)
        { }
    }
}