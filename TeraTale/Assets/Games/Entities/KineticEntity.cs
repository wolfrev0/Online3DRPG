using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class KineticEntity : Entity
{
    protected Animator _animator;
    protected SkinnedMeshRenderer _skimesh;
    public float AppearTime;
    public float disappearTime;

    new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _skimesh = GetComponentInChildren<SkinnedMeshRenderer>();
        Invoke("Appear", Random.Range(1, AppearTime));
        Invoke("Disappear",Random.Range(AppearTime,disappearTime));
    }
    
    void OnTriggerEnter()
    {
        Disappear();
    }

    protected abstract void Appear();
    protected abstract void Disappear();
}