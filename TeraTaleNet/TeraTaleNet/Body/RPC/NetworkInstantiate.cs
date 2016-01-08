namespace TeraTaleNet
{
    public class NetworkInstantiate : RPC
    {
        public string pfName;
        public int networkID;

        public NetworkInstantiate(string pfName)
            : base(RPCType.AllBuffered)
        {
            this.pfName = pfName;
        }

        public NetworkInstantiate(byte[] data)
            : base(data)
        { }
    }
}