
using TeraTaleNet;
using UnityEngine;
using UnityEngine.UI;

public class BuyDialog : MonoBehaviour
{
    static public BuyDialog instance;
    public Image image;
    public Text buyPrice;
    public InputField input;
    private Item _item;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Open(Item item)
    {
        _item = item;
        image.sprite = item.sprite;
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
            //BuyItem
        }
        else
        {
            //Show Error Message
        }
        Close();
    }
}
