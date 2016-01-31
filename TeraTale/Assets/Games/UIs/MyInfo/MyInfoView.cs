using UnityEngine;
using UnityEngine.EventSystems;

public class MyInfoView : MonoBehaviour, IDragHandler
{
    public Camera playerBodyCamera;
    float theta = 0;

    void OnEnable()
    {
        playerBodyCamera.enabled = true;
        playerBodyCamera.transform.SetParent(Player.mine.transform);
    }

    void OnDisable()
    {
        playerBodyCamera.enabled = false;
    }

    void Update()
    {
        playerBodyCamera.transform.localPosition = Quaternion.Euler(0, theta, 0) * new Vector3(0, 1f, -0.9f);
        playerBodyCamera.transform.localEulerAngles = new Vector3(0, theta, 0);
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            theta += eventData.delta.x;
    }
}
