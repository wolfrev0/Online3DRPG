using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    AudioSource _audio;
    RectTransform _rt;
    Vector2 _destination;
    Vector2 _initPos;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _rt = GetComponent<RectTransform>();
        _initPos = _rt.anchoredPosition;
        _destination = _initPos;
    }

    public void Show()
    {
        _destination = Vector2.zero;
        GlobalSound.instance.GetComponent<AudioSource>().enabled = false;
        _audio.volume = GlobalOption.instance.bgmVolume;
        _audio.Play();
    }

    public void Hide()
    {
        _destination = _initPos;
        GlobalSound.instance.Reset();
        _audio.Stop();
    }

    void Update()
    {
        _rt.anchoredPosition *= 9;
        _rt.anchoredPosition += _destination;
        _rt.anchoredPosition /= 10;
    }
}