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

    float bgmVolume
    {
        get { return GlobalOption.instance.bgmVolume; }
        set { GlobalOption.instance.bgmVolume = value; }
    }
    public float effectVolume
    {
        get { return GlobalOption.instance.effectVolume; }
        set { GlobalOption.instance.effectVolume = value; }
    }

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        instance = this;
        PlayBGM();
    }

    void PlayBGM()
    {
        var clip = bgms[Random.Range(0, bgms.Length)];
        _audio.PlayOneShot(clip, bgmVolume);
        Invoke("PlayBGM", clip.length);
    }

    public void PlayDie()
    {
        _audio.PlayOneShot(die, effectVolume);
    }

    public void PlayItemPick()
    {
        _audio.PlayOneShot(itemPick, effectVolume);
    }

    public void PlayNPCHello()
    {
        _audio.PlayOneShot(npcHello, effectVolume);
    }

    public void PlayNPCBye()
    {
        _audio.PlayOneShot(npcBye, effectVolume);
    }

    public void PlayNPCThanks()
    {
        _audio.PlayOneShot(npcThanks, effectVolume);
    }

    public void PlayCashRegister()
    {
        _audio.PlayOneShot(cashRegister, effectVolume);
    }

    public void PlayPickaxe()
    {
        _audio.PlayOneShot(pickaxes[Random.Range(0, pickaxes.Length)], effectVolume);
    }

    public void Reset()
    {
        _audio.enabled = false;
        _audio.enabled = true;
        CancelInvoke();
        PlayBGM();
    }

    public void SetBgmSize(float size)
    {
        bgmVolume = size;
        Reset();
    }

    public void SetEffectSize(float size)
    {
        effectVolume = size;
    }
}