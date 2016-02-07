using UnityEngine.EventSystems;

public class ShopSlot : ItemSlot
{

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
            if (itemSlot is InventorySlot)
            {
                //Sell Dialog
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
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