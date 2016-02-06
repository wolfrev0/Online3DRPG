using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    static ItemSlotPopupView _popup = null;
    int _itemStackIndex = -1;
    Image _image;
    public Text _count;
    public Text _equipState;
    Vector3 _alignedPosition;
    RectTransform _rt;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        if (!_popup)
            _popup = FindObjectOfType<ItemSlotPopupView>();
    }

    void Start()
    {
        _alignedPosition = _rt.anchoredPosition;
    }

    public void SetItemStack(int itemStackIndex)
    {
        _itemStackIndex = itemStackIndex;
    }

    void Update()
    {
        ItemStack itemStack = Player.mine.itemStacks[_itemStackIndex];
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Player.mine.Send(new ItemUse(_itemStackIndex));
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rt, Input.mousePosition, Camera.main, out pos);
            _rt.position = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();

        if(eventData.pointerCurrentRaycast.gameObject == null)
            Player.mine.DropItemStack(_itemStackIndex);
    }

    public void ResetPosition()
    {
        _rt.anchoredPosition = _alignedPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Player.mine.SwapItemStack(_itemStackIndex, eventData.pointerDrag.GetComponent<ItemSlot>()._itemStackIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(true);
        _popup.item = Player.mine.itemStacks[_itemStackIndex].item;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(false);
    }
}