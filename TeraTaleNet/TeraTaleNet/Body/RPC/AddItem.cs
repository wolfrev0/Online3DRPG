namespace TeraTaleNet
{
    public class AddItem : RPC
    {
        public Packet item;

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