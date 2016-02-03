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
        if (enabled == false)
            return;
        if (coll.tag == "Player")
        {
            if (observer.target == null)
                observer.AddTarget(coll.GetComponent<Player>());
            observer.Chase();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (enabled == false)
            return;
        if (observer.target != null && coll.gameObject == observer.target.gameObject)
            observer.RemoveTarget(coll.GetComponent<Player>());
    }
}