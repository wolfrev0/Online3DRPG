namespace TeraTaleNet
{
    public class BackTumbling : RPC
    {
        public BackTumbling()
            : base(RPCType.All)
        { }

        public BackTumbling(byte[] data)
            : base(data)
        { }
    }
}