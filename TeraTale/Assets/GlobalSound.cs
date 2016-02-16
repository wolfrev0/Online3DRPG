using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    static public GlobalSound instance;
    public AudioClip die;
    public AudioClip itemPick;

    AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlayDie()
    {
        _audio.PlayOneShot(die);
    }

    public void PlayItemPick()
    {
        _audio.PlayOneShot(itemPick);
    }
}