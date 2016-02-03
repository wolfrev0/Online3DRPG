using UnityEngine;
using System.Collections;

public class AttackRangeDetector : MonoBehaviour
{
    Enemy observer;

    void Awake()
    {
        observer = GetComponentInParent<Enemy>();
    }

    void OnTriggerStay(Collider coll)
    {
        if (enabled == false)
            return;
        if (observer.target != null && coll.gameObject == observer.target.gameObject)
        {
            if (observer.CanAttackTarget())
                observer.Attack();
            else
                observer.Chase();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (enabled == false)
            return;
        if (observer.target != null && coll.gameObject == observer.target.gameObject)
            observer.Chase();
    }
}