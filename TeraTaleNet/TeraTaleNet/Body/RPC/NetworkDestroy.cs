namespace TeraTaleNet
{
    public class NetworkDestroy : RPC
    {
        public int networkID;

        public NetworkDestroy(RPCType rpcType, int networkID)
            : base(rpcType)
        {
            this.networkID = networkID;
        }

        public NetworkDestroy(byte[] data)
            : base(data)
        { }
    }
}