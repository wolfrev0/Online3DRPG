using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public Canvas canvas;
    public Text text;
    public string targetWorld;

    void Start()
    {
        text.text = targetWorld + "로 이동합니다.";
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject == Player.mine.gameObject)
            canvas.enabled = true;
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject == Player.mine.gameObject)
            canvas.enabled = false;
    }

    public void SwitchWorld()
    {
        Player.mine.SwitchWorld(targetWorld);
    }
}