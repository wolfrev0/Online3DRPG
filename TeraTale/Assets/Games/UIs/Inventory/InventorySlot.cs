using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using TeraTaleNet;

public class InventorySlot : ItemSlot
{
    public override void OnBeginDrag(PointerEventData eventData)
    { }

    public override void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, Camera.main, out pos);
            rt.position = pos;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();

        if (eventData.pointerCurrentRaycast.gameObject == null)
            Player.mine.DropItemStack(itemStackIndex);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Player.mine.SwapItemStack(itemStackIndex, eventData.pointerDrag.GetComponent<ItemSlot>().itemStackIndex);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Use();
    }

    public void Use()
    {
        Player.mine.Use(itemStackIndex);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(true);
        _popup.item = Player.mine.itemStacks[itemStackIndex].item;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(false);
    }
}