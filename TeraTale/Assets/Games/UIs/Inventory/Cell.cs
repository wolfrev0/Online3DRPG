using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public class Cell : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    static CellPopupView _popup;
    int _itemStackIndex = -1;
    ItemStack _itemStack;
    Image _image;
    public Text _count;
    public Text _equipState;
    Vector3 _alignedPosition;

    void Awake()
    {
        _image = GetComponent<Image>();
        if (!_popup)
            _popup = FindObjectOfType<CellPopupView>();
    }

    void Start()
    {
        _alignedPosition = transform.position;
    }

    public void SetItemStack(ItemStack itemStack, int itemStackIndex)
    {
        Awake();//이상한 null 버그 땜빵..
        _itemStack = itemStack;
        _itemStackIndex = itemStackIndex;
    }

    void Update()
    {
        _image.sprite = _itemStack.sprite;
        _count.text = _itemStack.count.ToString();
        if (_itemStack.count <= 1)
            _count.text = "";

        var equipment = _itemStack.item as Equipment;
        if (equipment != null)
            _equipState.gameObject.SetActive(Player.mine.IsEquiping(equipment));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Player.mine.Send(new ItemUse(_itemStackIndex));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if(eventData.button == PointerEventData.InputButton.Left)
        //{
        //    transform.position = eventData.position;
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //ResetPosition();
    }

    public void ResetPosition()
    {
        //transform.position = _alignedPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    var tmp = eventData.pointerDrag.GetComponent<Cell>()._itemStack;
        //    eventData.pointerDrag.GetComponent<Cell>()._itemStack = _itemStack;
        //    _itemStack = tmp;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _popup.gameObject.SetActive(false);
    }
}