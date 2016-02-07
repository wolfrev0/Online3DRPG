using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeraTaleNet;

public abstract class ItemSlot : MonoBehaviour, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemStackIndex = -1;
    protected RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnDrop(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}