using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    static public GlobalSound instance;
    public AudioClip die;
    public AudioClip itemPick;
    public AudioClip npcHello;
    public AudioClip npcBye;
    public AudioClip npcThanks;
    public AudioClip cashRegister;
    public AudioClip[] pickaxes;
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

    public void PlayNPCHello()
    {
        _audio.PlayOneShot(npcHello);
    }

    public void PlayNPCBye()
    {
        _audio.PlayOneShot(npcBye);
    }

    public void PlayNPCThanks()
    {
        _audio.PlayOneShot(npcThanks);
    }

    public void PlayCashRegister()
    {
        _audio.PlayOneShot(cashRegister);
    }

    public void PlayPickaxe()
    {
        _audio.PlayOneShot(bgms[Random.Range(0, pickaxes.Length)]);
    }
}