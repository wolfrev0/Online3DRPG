using TeraTaleNet;
using UnityEngine.UI;

public class BuyDialog : Modal
{
    static public BuyDialog instance;
    public Image image;
    public Text buyPrice;
    public InputField input;
    private Item _item;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(Item item)
    {
        _item = item;
        image.sprite = item.sprite;
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
        buyPrice.text = _item.price * amount + "G";
    }

    public void Buy()
    {
        var amount = int.Parse(input.text);
        if(Player.mine.money >= _item.price*amount && Player.mine.CanAddItem(_item, amount))
        {
            Player.mine.BuyItem(_item, amount);
        }
        else
        {
            //Show Error Message
        }
        Close();
    }
}
