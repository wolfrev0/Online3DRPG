namespace TeraTaleNet
{
    public class AddItem : RPC
    {
        public ISerializable item;

        public AddItem(string receiver, Item item)
            : base(RPCType.Specific, receiver)
        {
            this.item = item;
        }

        public AddItem(byte[] data)
            : base(data)
        { }
    }
}