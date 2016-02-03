namespace TeraTaleNet
{
    public class AddItem : RPC
    {
        public IAutoSerializable item;

        public AddItem(Item item)
            : base(RPCType.All)
        {
            this.item = item;
        }

        public AddItem()
            : base(RPCType.All)
        { }
    }
}