using UnityEngine;

public abstract class KineticEntity : Entity
{
    protected Animator _animator;
    public float disappearTime;

    protected new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        Appear();
        Invoke("Disappear", disappearTime);
    }
    
    void OnColliderEnter()
    {
        Disappear();
    }

    protected abstract void Appear();
    protected abstract void Disappear();
}