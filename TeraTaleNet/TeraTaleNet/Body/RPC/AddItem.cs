namespace TeraTaleNet
{
    public class AddItem : RPC
    {
        public Item item;

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