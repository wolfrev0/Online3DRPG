using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    ItemStack _itemStack;
    Image _image;
    [SerializeField] Text _count;
    [SerializeField] Text _equipState;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetItemStack(ItemStack itemStack)
    {
        Awake();//이상한 null 버그 땜빵..
        _itemStack = itemStack;
    }

    void Update()
    {
        _image.sprite = _itemStack.sprite;
        _count.text = _itemStack.count.ToString();
        if (_itemStack.count <= 1)
            _count.text = "";

        var equipment = _itemStack.item as Equipment;
        if (equipment != null)
            _equipState.gameObject.SetActive(Player.FindPlayerByName(NetworkScript.userName).IsEquiping(equipment));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _itemStack.Use();
        }
    }
}