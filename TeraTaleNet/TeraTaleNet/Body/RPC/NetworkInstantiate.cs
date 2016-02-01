namespace TeraTaleNet
{
    public class NetworkInstantiate : RPC
    {
        public int networkID;
        public string pfName;
        public IAutoSerializable callbackArg;
        public string callback;

        public NetworkInstantiate(string pfName, IAutoSerializable callbackArg, string callback)
            : base(RPCType.AllBuffered)
        {
            this.pfName = pfName;
            this.callbackArg = callbackArg;
            this.callback = callback;
        }

        public NetworkInstantiate()
            : base(RPCType.AllBuffered)
        { }
    }
}