using UnityEngine;
using TeraTaleNet;

//Target Detect 알고리즘
//1. 처음 타겟을 쫓아간다.
//2. 그후에 딜량체크해서 딜 많이넣은 플레이어 쫓아간다.
public abstract class Enemy : AliveEntity
{
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    TargetDetector _targetDetector;

    AliveEntity _target;
    public AliveEntity target
    {
        get { return _target; }
        set
        {
            if (isServer && _target != value)
            {
                int targetSignallerID = 0;
                if (value)
                    targetSignallerID = value.networkID;

                var rpc = new Chase(targetSignallerID);
                Chase(rpc);
                Send(rpc);
            }
        }
    }

    public void Chase(Chase rpc)
    {
        bool flag = false;
        _target = null;
        if(rpc.targetSignallerID !=0)
        {
            flag = true;
            _target = (AliveEntity)NetworkProgramUnity.currentInstance.signallersByID[rpc.targetSignallerID];
        }
        _navMeshAgent.enabled = flag;
        _animator.SetBool("Chase", flag);
    }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _targetDetector = GetComponentInChildren<TargetDetector>();
    }

    protected void Update()
    {
        if (_target && _navMeshAgent.enabled)
            _navMeshAgent.destination = _target.transform.position;
    }

    public void Attack()
    {
        _animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        _animator.SetBool("Attack", false);
    }
}