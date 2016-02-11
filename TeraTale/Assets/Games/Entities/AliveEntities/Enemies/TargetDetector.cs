using UnityEngine;

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
            if (!observer.mainTarget)
                observer.AddTarget(coll.GetComponent<Player>());
            observer.Chase();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (enabled == false)
            return;
        if (coll.tag == "Player")
        {
            if (observer.ContainsTarget(coll.gameObject))
                observer.RemoveTarget(coll.GetComponent<Player>());
        }
    }
}