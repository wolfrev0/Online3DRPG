using UnityEngine;

public class FireBreath : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem smoke;
    AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void On()
    {
        fire.Play();
        smoke.Play();
        _audio.Play();
        _audio.volume = GlobalSound.instance.effectVolume;
    }

    public void Off()
    {
        fire.Stop();
        smoke.Stop();
        _audio.Stop();
    }
}