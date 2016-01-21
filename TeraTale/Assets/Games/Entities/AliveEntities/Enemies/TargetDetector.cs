using UnityEngine;
using System.Collections;

public class TargetDetector : MonoBehaviour
{
    Enemy observer;

    void Awake()
    {
        observer = transform.GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (NetworkScript.isServer && observer.target == null && coll.tag == "Player")
            observer.Chase(coll.GetComponent<Player>());
    }

    void OnTriggerExit(Collider coll)
    {
        if (NetworkScript.isServer && observer.target != null && coll.gameObject == observer.target.gameObject)
            observer.ChaseStop();
    }
}