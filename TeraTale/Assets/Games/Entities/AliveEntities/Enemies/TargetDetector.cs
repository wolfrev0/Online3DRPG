using UnityEngine;
using System.Collections;

public class TargetDetector : MonoBehaviour
{
    Enemy observer;

    void Awake()
    {
        observer = transform.GetComponentInParent<Enemy>();
    }

    void OnTriggerStay(Collider coll)
    {
        if (NetworkScript.isServer && observer.target == null && coll.tag == "Player")
            observer.target = coll.GetComponent<Player>();
    }

    void OnTriggerExit(Collider coll)
    {
        if (NetworkScript.isServer && coll.gameObject == observer.target.gameObject)
            observer.target = null;
    }
}