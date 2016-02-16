using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    static public GlobalSound instance;
    public AudioClip die;
    public AudioClip itemPick;
    public AudioClip[] bgms;

    AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        instance = this;
        PlayBGM();
    }

    public void PlayDie()
    {
        _audio.PlayOneShot(die);
    }

    public void PlayItemPick()
    {
        _audio.PlayOneShot(itemPick);
    }

    void PlayBGM()
    {
        var clip = bgms[Random.Range(0, bgms.Length)];
        _audio.PlayOneShot(clip, 0.7f);
        Invoke("PlayBGM", clip.length);
    }
}