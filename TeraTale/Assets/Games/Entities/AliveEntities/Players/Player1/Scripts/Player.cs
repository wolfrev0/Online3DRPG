using UnityEngine;
using TeraTaleNet;

public class Player : AliveEntity
{
    const float kRaycastDistance = 50.0f;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    NetworkSignaller _net;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _net = GetComponent<NetworkSignaller>();
    }

    void Update()
    {
    }

    public void HandleInput()
    {
        if (!_net.isMine)
            return;

        if (Input.GetButtonDown("Move"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, kRaycastDistance))
            {
                _net.SendRPC(new Navigate(RPCType.All, hit.point.x, hit.point.y, hit.point.z));
            }
        }

        if (Input.GetButtonDown("Attack"))
        {
    //        _net.SendRPC(new Attack(RPCType.All));
        }
    }

    //public void Attack(Attack info)
    //{
    //    _animator.SetTrigger("Attacking");
    //}

    void Die()
    {
        _animator.SetTrigger("Dying");
    }

    public bool IsArrived()
    {
        if (_navMeshAgent.enabled == false)
            return true;
        var toDestination = _navMeshAgent.destination - transform.position;
        return toDestination.magnitude <= _navMeshAgent.stoppingDistance;
    }

    public void Navigate(Navigate info)
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.destination = new Vector3(info.x, info.y, info.z);
        _animator.SetBool("Running", true);
    }

    public void NavigateStop()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Running", false);
    }
}