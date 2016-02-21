using TeraTaleNet;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotPopupView : MonoBehaviour
{
    static public ItemSlotPopupView instance;

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
            nameText.text = value.ingameName;
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
        instance = this;
        _rt = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            _rt.position = new Vector2(-999, -999);
        else
            _rt.position = Input.mousePosition;
        if (_rt.position.y > Screen.height / 2)
            _rt.pivot = new Vector2(0, 1);
        else
            _rt.pivot = new Vector2(0, 0);
    }
}