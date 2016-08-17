using UnityEngine;
using UnityEngine.UI;

public class OptionDialog : MonoBehaviour
{
    public Scrollbar bgm;
    public Scrollbar effect;

    void OnEnable()
    {
        bgm.value = GlobalOption.instance.bgmVolume;
        effect.value = GlobalOption.instance.effectVolume;
    }
}