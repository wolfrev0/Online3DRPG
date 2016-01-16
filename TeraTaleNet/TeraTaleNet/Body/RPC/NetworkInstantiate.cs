namespace TeraTaleNet
{
    public class NetworkInstantiate : RPC
    {
        public int networkID;
        public string pfName;
        public Packet callbackArg;
        public string callback;

        public NetworkInstantiate(string pfName, Packet callbackArg, string callback)
            : base(RPCType.AllBuffered)
        {
            this.pfName = pfName;
            this.callbackArg = callbackArg;
            this.callback = callback;
        }

        public NetworkInstantiate(byte[] data)
            : base(data)
        { }
    }
}