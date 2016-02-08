using UnityEngine;
using TeraTaleNet;
using UnityEngine.UI;

public class ScrollIngredientView : MonoBehaviour
{
    static public ScrollIngredientView instance;

    public Text nameView;
    public Image icon;
    public IngredientSlot[] ingredientSlots;
    Scroll _scroll;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(Scroll scroll)
    {
        gameObject.SetActive(true);
        _scroll = scroll;
        nameView.text = scroll.name;
        icon.sprite = scroll.output.sprite;

        foreach (var ingredientSlot in ingredientSlots)
            ingredientSlot.Hide();
        var ingredients = scroll.ingredients;
        for (int i=0;i<ingredients.Count;i++)
            ingredientSlots[i].Reset(ingredients[i]);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Combine()
    {
        var ingredients = _scroll.ingredients;

        for (int i = 0; i < ingredients.Count; i++)
            if (ingredientSlots[i].isSatisfied == false)
                return;

        for (int i = 0; i < ingredients.Count; i++)
        {
            Player.mine.RemoveItem(ingredients[i].item, ingredients[i].count);
        }
        Player.mine.AddItem(_scroll.output);
        Close();
    }
}
