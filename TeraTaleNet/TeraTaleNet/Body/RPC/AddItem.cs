namespace TeraTaleNet
{
    public class AddItem : RPC
    {
        public IAutoSerializable item;

        public AddItem(string receiver, Item item)
            : base(RPCType.Specific, receiver)
        {
            this.item = item;
        }

        public AddItem()
        { }
    }
}