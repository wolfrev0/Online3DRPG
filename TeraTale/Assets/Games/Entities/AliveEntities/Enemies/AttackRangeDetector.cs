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
        if (observer.target != null && coll.gameObject == observer.target.gameObject)
            observer.Attack();
    }

    IEnumerator OnTriggerExit(Collider coll)
    {
        while (observer.target == null)
            yield return null;
        if (coll.gameObject == observer.target.gameObject)
            observer.StopAttack();
    }
}