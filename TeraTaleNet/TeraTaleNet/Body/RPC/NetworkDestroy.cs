namespace TeraTaleNet
{
    public class NetworkDestroy : RPC
    {
        public int networkID;

        public NetworkDestroy(int networkID)
            : base(RPCType.Others)
        {
            this.networkID = networkID;
        }

        public NetworkDestroy()
        { }
    }
}