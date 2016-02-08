using TeraTaleNet;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : ItemSlot
{
    Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (NPCShop.instance && NPCShop.instance.currentOwner)
        {
            ItemStack itemStack = NPCShop.instance.currentOwner.itemStacks[itemStackIndex];
            _image.sprite = itemStack.sprite;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if (Player.mine.itemStacks[itemSlot.itemStackIndex].item.isNull)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
            SellDialog.instance.Open(itemSlot.itemStackIndex);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (NPCShop.instance.currentOwner.itemStacks[itemStackIndex].item.isNull)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
            BuyDialog.instance.Open(NPCShop.instance.currentOwner.itemStacks[itemStackIndex].item);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ItemSlotPopupView.instance.gameObject.SetActive(true);
        ItemSlotPopupView.instance.item = NPCShop.instance.currentOwner.itemStacks[itemStackIndex].item;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        ItemSlotPopupView.instance.gameObject.SetActive(false);
    }
}