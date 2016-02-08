using TeraTaleNet;

public class BuyItem : RPC
{
    public Item item;
    public int amount;

    public BuyItem(string receiver, Item item, int amount)
        : base(RPCType.Specific, receiver)
    {
        this.item = item;
        this.amount = amount;
    }

    public BuyItem()
        : base(RPCType.Specific)
    { }
}