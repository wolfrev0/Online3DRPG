namespace TeraTaleNet
{
    public class SwapItemStack : RPC
    {
        public int indexA;
        public int indexB;

        public SwapItemStack(int indexA, int indexB)
            : base(RPCType.All)
        {
            this.indexA = indexA;
            this.indexB = indexB;
        }

        public SwapItemStack()
            : base(RPCType.All)
        { }
    }
}