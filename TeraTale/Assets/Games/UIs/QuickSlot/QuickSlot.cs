using UnityEngine.EventSystems;

public class QuickSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            itemStackIndex = eventData.pointerDrag.GetComponent<ItemSlot>().itemStackIndex;
    }
}