using UnityEngine;
using TeraTaleNet;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

//Target Detect 알고리즘
//1. 처음 타겟을 쫓아간다.
//2. 그후에 딜량체크해서 딜 많이넣은 플레이어 쫓아간다.
//붙어있는 타겟 처리 알고리즘
//공격시에는 NavMeshAgent의 updateRotation=false로 하자.
//공격 후 target에게 Raycast하여 false일경우 이동&updateRotation=true하고 true일경우 그대로 공격
public abstract class Enemy : AliveEntity
{
    public AttackSubject _attackSubject;
    public Text nameView;
    public Item[] items;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    
    public AliveEntity target
    { get; set; }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected new void Start()
    {
        base.Start();
        nameView.text = name;
    }

    void AttackBegin()
    {
        _attackSubject.enabled = true;
    }

    void AttackEnd()
    {
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
        _animator.SetBool("Chase", flag);
    }

    public void ChaseStop()
    {
        Chase(null as AliveEntity);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public bool CanAttackTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, float.MaxValue))
        {
            var root = hit.transform.root;
            if (root.tag == "Player")
            {
                var player = root.GetComponent<Player>();
                if (player == target)
                    return true;
            }
        }
        return false;
    }

    protected abstract List<Item> DropItems
    { get; }

    protected override void Die()
    {
        _animator.SetTrigger("Die");
        if (isServer)
            foreach (var item in DropItems)
                NetworkInstantiate(item.solidPrefab.GetComponent<NetworkScript>(), item, "OnDropItemInstantiate");
        Destroy(gameObject, 5);
    }

    public void OnDropItemInstantiate(ItemSolid item)
    {
        item.transform.position = transform.position + Vector3.up;
    }
}