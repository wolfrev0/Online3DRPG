using UnityEngine.EventSystems;

public class ShopSlot : ItemSlot
{

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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Buy Dialog
        }
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