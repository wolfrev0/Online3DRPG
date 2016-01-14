using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    ItemStack _itemStack;
    Image _image;
    Text _text;

    void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
    }

    public void SetItemStack(ItemStack itemStack)
    {
        if(_image == null)
            _image = GetComponent<Image>();
        if(_text == null)
            _text = GetComponentInChildren<Text>();
        _itemStack = itemStack;
        Renew();
    }

    public void Renew()
    {
        _image.sprite = _itemStack.sprite;
        _text.text = _itemStack.count.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _itemStack.Use();
            Renew();
        }
    }
}
