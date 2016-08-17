using UnityEngine;
using UnityEngine.UI;
using TeraTaleNet;

public class IngredientSlot : MonoBehaviour
{
    public Image icon;
    public Text needAmount;
    public Text haveAmount;
    bool _isSatisfied = false;
    Image img;

    public void Hide()
    {
        gameObject.SetActive(false);
        img = GetComponent<Image>();
    }

    public void Reset(Scroll.Ingradient ingredient)
    {
        gameObject.SetActive(true);
        icon.sprite = ingredient.item.sprite;
        var need = ingredient.count;
        var have = Player.mine.ItemCount(ingredient.item);
        needAmount.text = need.ToString();
        haveAmount.text = have.ToString();
        if (have >= need)
        {
            _isSatisfied = true;
            img.color = new Color(0, 200, 255, 255);
        }
        else
        {
            _isSatisfied = false;
            img.color = new Color(255, 0, 10, 255);
        }
    }

    public bool isSatisfied { get { return _isSatisfied; } }
}