namespace TeraTaleNet
{
    public class DropItemStack : RPC
    {
        public int index;

        public DropItemStack(int index)
            : base(RPCType.All)
        {
            this.index = index;
        }

        public DropItemStack()
            : base(RPCType.All)
        { }
    }
}