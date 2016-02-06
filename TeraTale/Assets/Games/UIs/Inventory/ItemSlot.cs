using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    static ItemSlotPopupView _popup = null;
    public int itemStackIndex = -1;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Use();
        }
    }

    public void Use()
    {
        Player.mine.Send(new ItemUse(itemStackIndex));
    }

    public void OnBeginDrag(PointerEventData eventData)
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
            Player.mine.DropItemStack(itemStackIndex);
    }

    void ResetPosition()
    {
        _rt.anchoredPosition = _alignedPosition;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Player.mine.SwapItemStack(itemStackIndex, eventData.pointerDrag.GetComponent<ItemSlot>().itemStackIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(true);
        _popup.item = Player.mine.itemStacks[itemStackIndex].item;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(false);
    }
}