using UnityEngine;
using UnityEngine.UI;
using TeraTaleNet;

public class IngredientSlot : MonoBehaviour
{
    public Image icon;
    public Text needAmount;
    public Text haveAmount;
    bool _isSatisfied = false;

    public void Hide()
    {
        gameObject.SetActive(false);
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
            _isSatisfied = true;
        else
            _isSatisfied = false;
    }

    public bool isSatisfied { get { return _isSatisfied; } }
}