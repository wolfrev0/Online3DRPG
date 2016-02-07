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
        if (coll.tag == "Player")
        {
            if (observer.ContainsTarget(coll.gameObject))
            {
                if (observer.CanAttackTarget())
                    observer.Attack();
                else
                    observer.Chase();
            }
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (enabled == false)
                return;
            if (observer.ContainsTarget(coll.gameObject))
                observer.Chase();
        }
    }
}