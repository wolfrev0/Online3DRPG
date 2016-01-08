namespace TeraTaleNet
{
    public class NetworkInstantiate : RPC
    {
        public string pfName;
        public int networkID;

        public NetworkInstantiate(RPCType rpcType, string pfName)
            : base(rpcType)
        {
            this.pfName = pfName;
        }

        public NetworkInstantiate(byte[] data)
            : base(data)
        { }
    }
}