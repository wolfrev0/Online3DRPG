using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Animator _animator;

    public Vector3 destination
    {
        set { _navMeshAgent.destination = value; }
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    void Attack()
    {
        _animator.SetTrigger("Attacking");
    }

    void Die()
    {
        _animator.SetTrigger("Dying");
    }
}