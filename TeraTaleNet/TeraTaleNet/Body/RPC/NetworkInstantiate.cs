namespace TeraTaleNet
{
    public class NetworkInstantiate : RPC
    {
        public int index;
        public int networkID;

        public NetworkInstantiate(RPCType rpcType, int index)
            : base(rpcType)
        {
            this.index = index;
        }

        public NetworkInstantiate(byte[] data)
            : base(data)
        { }
    }
}