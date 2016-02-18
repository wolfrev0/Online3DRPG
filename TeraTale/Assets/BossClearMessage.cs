using UnityEngine;

public class BossClearMessage : MonoBehaviour
{
    static public BossClearMessage instance;
    RectTransform _rt;
    Vector2 _destination;
    Vector2 _initPos;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _initPos = _rt.anchoredPosition;
        _destination = _initPos;
        instance = this;
        Show();
    }

    public void Show()
    {
        _destination = _rt.anchoredPosition + Vector2.right * 1600;
        Invoke("Hide", 5);
    }

    void Update()
    {
        _rt.anchoredPosition *= 9;
        _rt.anchoredPosition += _destination;
        _rt.anchoredPosition /= 10;
    }

    void Hide()
    {
        _destination = _rt.anchoredPosition + Vector2.right * 1600;
        Invoke("Reset", 3);
    }

    void Reset()
    {
        _rt.anchoredPosition = _initPos;
        _destination = _initPos;
    }
}