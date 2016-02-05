using UnityEngine;

public class CellPopupView : MonoBehaviour
{
    RectTransform _rt;

    void Start()
    {
        gameObject.SetActive(false);
        _rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        _rt.position = Input.mousePosition;
    }
}