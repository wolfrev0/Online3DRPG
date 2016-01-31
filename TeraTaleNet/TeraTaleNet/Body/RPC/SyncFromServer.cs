namespace TeraTaleNet
{
    public class SyncFromServer : RPC
    {
        public string member;
        public Packet packet = new NullPacket();

        public SyncFromServer(string member)
            : base(RPCType.Others)
        {
            this.member = member;
        }

        public SyncFromServer()
        { }
    }
}