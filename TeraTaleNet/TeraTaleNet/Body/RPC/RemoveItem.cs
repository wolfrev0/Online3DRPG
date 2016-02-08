namespace TeraTaleNet
{
    public class RemoveItem : RPC
    {
        public Item item;
        public int amount;

        public RemoveItem(Item item, int amount)
            : base(RPCType.All)
        {
            this.item = item;
            this.amount = amount;
        }

        public RemoveItem()
            : base(RPCType.All)
        { }
    }
}