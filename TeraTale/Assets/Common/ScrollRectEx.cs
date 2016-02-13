using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectEx : ScrollRect
{
    protected new void Awake()
    {
        base.Awake();
    }

    public new void SetContentAnchoredPosition(Vector2 anchoredPos)
    {
        base.SetContentAnchoredPosition(anchoredPos);
    }

    public new void SetDirty()
    {
        base.SetDirty();
        OnScroll(new PointerEventData(null));
    }
}