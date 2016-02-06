using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class QuickSlot : ItemSlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            itemStackIndex = eventData.pointerDrag.GetComponent<ItemSlot>().itemStackIndex;
    }
}