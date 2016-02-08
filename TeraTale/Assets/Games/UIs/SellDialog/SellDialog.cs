using UnityEngine.UI;

public class SellDialog : Modal
{
    static public SellDialog instance;
    public Image image;
    public Text sellPrice;
    public InputField input;
    private int _itemStackIndex;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(int itemStackindex)
    {
        _itemStackIndex = itemStackindex;
        image.sprite = Player.mine.itemStacks[itemStackindex].sprite;
        input.text = "0";
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void RenewPriceText(string amountStr)
    {
        var amount = int.Parse(amountStr);
        sellPrice.text = Player.mine.itemStacks[_itemStackIndex].item.price * amount + "G";
    }

    public void Sell()
    {
        var amount = int.Parse(input.text);
        if (Player.mine.itemStacks[_itemStackIndex].count >= amount)
            Player.mine.SellItem(_itemStackIndex, amount);
        else
        {
            //Show Error Message
        }
        Close();
    }
}
