using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public abstract class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    static protected ItemSlotPopupView _popup = null;
    public int itemStackIndex = -1;
    Image _image;
    public Text _count;
    public Text _equipState;
    Vector3 _alignedPosition;
    protected RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        if (!_popup)
            _popup = FindObjectOfType<ItemSlotPopupView>();
    }

    void Start()
    {
        _alignedPosition = rt.anchoredPosition;
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
    
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);

    protected void ResetPosition()
    {
        rt.anchoredPosition = _alignedPosition;
    }

    public abstract void OnDrop(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}