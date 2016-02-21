using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class KineticEntity : MonoBehaviour
{
    protected Animator _animator;
    protected SkinnedMeshRenderer _skimesh;

    protected void Start()
    {
        _animator = GetComponent<Animator>();
        _skimesh = GetComponentInChildren<SkinnedMeshRenderer>();
        Invoke("Appear", Random.Range(0f, 4f));
    }
    
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
            Disappear();
    }

    protected abstract void Appear();
    protected abstract void Disappear();
}