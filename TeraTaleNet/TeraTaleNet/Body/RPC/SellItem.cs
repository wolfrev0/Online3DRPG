using TeraTaleNet;

public class SellItem : RPC
{
    public int index;
    public int amount;

    public SellItem(int index, int amount)
        : base(RPCType.All)
    {
        this.index = index;
        this.amount = amount;
    }

    public SellItem()
        : base(RPCType.All)
    { }
}