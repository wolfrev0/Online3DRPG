using UnityEngine;
using UnityEngine.EventSystems;
using TeraTaleNet;
using UnityEngine.UI;

public class InventorySlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text _count;
    public Text _equipState;
    Image _image;
    InventorySlotLayoutGroup _layoutGroup;

    protected void Start()
    {
        _image = GetComponent<Image>();
        _layoutGroup = Inventory.instance.GetComponentInChildren<InventorySlotLayoutGroup>();
    }

    void Update()
    {
        if (Player.mine == null)
            return;
        ItemStack itemStack = Player.mine.itemStacks[itemStackIndex];
        _image.sprite = itemStack.sprite;
        _count.text = itemStack.count.ToString();
        if (itemStack.count <= 1)
            _count.text = "";

        var equipment = itemStack.item as Equipment;
        if (equipment != null)
            _equipState.gameObject.SetActive(Player.mine.IsEquiping(equipment));
        else
            _equipState.gameObject.SetActive(false);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _layoutGroup.enabled = false;
        QuickSlotController.instance.grid.enabled = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition + new Vector3(10, -10, 0);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _layoutGroup.enabled = true;
        _layoutGroup.SetDirty();
        QuickSlotController.instance.grid.enabled = true;

        if (eventData.pointerCurrentRaycast.gameObject == null)
            Player.mine.DropItemStack(itemStackIndex);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();

        if (eventData.button == PointerEventData.InputButton.Left)
            Player.mine.SwapItemStack(itemStackIndex, itemSlot.itemStackIndex);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Use();
    }

    public void Use()
    {
        Player.mine.Use(itemStackIndex);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ItemSlotPopupView.instance.gameObject.SetActive(true);
        ItemSlotPopupView.instance.item = Player.mine.itemStacks[itemStackIndex].item;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        ItemSlotPopupView.instance.gameObject.SetActive(false);
    }
}