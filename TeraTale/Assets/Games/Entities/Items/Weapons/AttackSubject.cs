using UnityEngine;
using System.Collections;
using TeraTaleNet;

public class AttackSubject : MonoBehaviour
{
    public string targetTag;
    public AliveEntity owner;
    Collider _collider;
    TrailRenderer _trail;

    void Awake()
    {
        owner = GetComponentInParent<AliveEntity>();
        _collider = GetComponent<Collider>();
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
        {
            var ae = coll.GetComponent<AliveEntity>();
            ae.Damage(new Damage(Damage.Type.Physical, owner.attackDamage, 0, false));
        }
    }
}