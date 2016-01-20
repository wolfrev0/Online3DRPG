using UnityEngine;
using TeraTaleNet;
using UnityEngine.UI;
using System;

//Target Detect 알고리즘
//1. 처음 타겟을 쫓아간다.
//2. 그후에 딜량체크해서 딜 많이넣은 플레이어 쫓아간다.
public abstract class Enemy : AliveEntity
{
    public Attacker _attackSubject;
    public Text nameView;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    bool _lookAtTarget = true;
    
    public AliveEntity target
    { get; private set; }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected new void Start()
    {
        base.Start();
        nameView.text = name;
        Sync("target");
    }
    protected override void OnSynced(Sync sync)
    {
        if(sync.member=="target")
        {
            if (target)
                Chase(target);
        }
    }

    protected void Update()
    {
        if (target)
        {
            if (_lookAtTarget && !_navMeshAgent.isOnNavMesh)
                transform.LookAt(target.transform);
            if (_navMeshAgent.enabled)
                _navMeshAgent.destination = target.transform.position;
        }
    }

    void AttackBegin()
    {
        _lookAtTarget = false;
        _attackSubject.enabled = true;
    }

    void AttackEnd()
    {
        _lookAtTarget = true;
        _attackSubject.enabled = false;
    }

    public void Chase(AliveEntity target)
    {
        if (isServer && this.target != target)
        {
            this.target = target;
            int targetSignallerID = 0;
            if (target)
                targetSignallerID = target.networkID;
            Send(new Chase(targetSignallerID));
        }
    }

    public void Chase(Chase rpc)
    {
        bool flag = false;
        target = null;
        if (rpc.targetSignallerID != 0)
        {
            flag = true;
            target = (AliveEntity)NetworkProgramUnity.currentInstance.signallersByID[rpc.targetSignallerID];
        }
        _navMeshAgent.enabled = flag;
        _animator.SetBool("Chase", flag);
    }

    public void ChaseStop()
    {
        Chase(null as AliveEntity);
    }

    public void Attack()
    {
        _animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        _animator.SetBool("Attack", false);
    }

    protected override void Die()
    {
        _animator.SetTrigger("Die");
    }
}