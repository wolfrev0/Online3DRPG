using UnityEngine.EventSystems;

public class QuickSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
            itemStackIndex = itemSlot.itemStackIndex;
        }
    }
}