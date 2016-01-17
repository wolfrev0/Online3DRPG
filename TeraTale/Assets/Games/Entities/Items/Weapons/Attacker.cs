using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour
{
    public AliveEntity owner;
    Collider collider;
    TrailRenderer trail;

    void Awake()
    {
        collider = GetComponent<Collider>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void OnEnable()
    {
        if (collider)
            collider.enabled = true;
        if (trail)
            trail.enabled = true;
    }

    void OnDisable()
    {
        if (collider)
            collider.enabled = false;
        if (trail)
            trail.enabled = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Enemy")
            Debug.Log("Hit");
    }
}
