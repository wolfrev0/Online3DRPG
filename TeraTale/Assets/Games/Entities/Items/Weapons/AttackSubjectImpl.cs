using UnityEngine;
using TeraTaleNet;

public class AttackSubjectImpl : AttackSubject
{
    Collider _collider;
    AudioSource _sound;
    TrailRenderer _trail;

    void Awake()
    {
        owner = GetComponentInParent<AliveEntity>();
        _collider = GetComponent<Collider>();
        _sound = GetComponent<AudioSource>();
        _trail = GetComponentInChildren<TrailRenderer>();
    }

    void OnTransformParentChanged()
    {
        owner = GetComponentInParent<AliveEntity>();
    }

    void OnEnable()
    {
        if (_collider)
            _collider.enabled = true;
        if (_trail)
            _trail.enabled = true;
        if (_sound)
        {
            _sound.Play();
            _sound.volume = GlobalSound.instance.effectVolume;
        }
    }

    void OnDisable()
    {
        if (_collider)
            _collider.enabled = false;
        if (_trail)
            _trail.enabled = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (NetworkScript.isLocal)
            return;
        if (coll.tag == targetTag)
            ApplyDamage(coll.GetComponent<AliveEntity>());
    }

    void OnParticleCollision(GameObject other)
    {
        if (NetworkScript.isLocal)
            return;
        if (other.tag == targetTag)
            ApplyDamage(other.GetComponent<AliveEntity>());
    }

    void ApplyDamage(AliveEntity target)
    {
        target.Damage(new Damage(Damage.Type.Physical, owner.weaponType, owner.owner, damageCalculator(owner.attackDamage), 0, knockdown));
    }
}