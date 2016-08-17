using UnityEngine;

public class GlobalOption : MonoBehaviour
{
    static public GlobalOption instance;
    public float bgmVolume = 1;
    public float effectVolume = 1;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}