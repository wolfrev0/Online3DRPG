using UnityEngine;

public class PlayerDieDialog : MonoBehaviour
{
    static public PlayerDieDialog instance;

    void Awake()
    {
        instance = this;
        Hide();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}