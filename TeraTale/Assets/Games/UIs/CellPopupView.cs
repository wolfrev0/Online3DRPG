using TeraTaleNet;
using UnityEngine;
using UnityEngine.UI;

public class CellPopupView : MonoBehaviour
{
    public Text nameText;
    public Text price;
    public Text itemType;
    public Text weaponType;
    public Text rank;
    public Text wearableLevel;
    public Text upgradedLevel;
    public Text durability;
    public Text effect;
    public Text itemExplanation;

    RectTransform _rt;

    public Item item
    {
        set
        {
            nameText.text = value.name;
            price.text = value.price + " G";
            itemType.text = value.itemType.ToString();
            weaponType.text = "";
            rank.text = "";
            wearableLevel.text = "";
            upgradedLevel.text = "";
            durability.text = "";
            effect.text = value.effectExplanation;
            itemExplanation.text = value.explanation;
            if (value.isNull)
                gameObject.SetActive(false);
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
        _rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        _rt.position = Input.mousePosition;
    }
}