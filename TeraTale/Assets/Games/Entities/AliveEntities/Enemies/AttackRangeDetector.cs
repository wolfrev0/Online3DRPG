using UnityEngine;
using System.Collections;

public class AttackRangeDetector : MonoBehaviour
{
    Enemy observer;

    void Awake()
    {
        observer = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter(Collider coll)
    {
        OnTriggerStay(coll);
    }

    void OnTriggerStay(Collider coll)
    {
        if (observer.target != null && coll.gameObject == observer.target.gameObject)
        {
            if (observer.CanAttackTarget())
                observer.Attack();
            else
                observer.Chase(observer.target);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        observer.Chase(observer.target);
    }
}