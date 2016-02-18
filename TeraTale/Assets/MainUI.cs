using UnityEngine;

public class MainUI : MonoBehaviour
{
    static public MainUI instance;

    void Awake()
    {
        instance = this;
    }
}
