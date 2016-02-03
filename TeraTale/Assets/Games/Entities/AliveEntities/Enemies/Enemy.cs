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
    public MonsterSpawner spawner { get; set; }
    Animator _animator;

    SortedList<float, AliveEntity> _targets = new SortedList<float, AliveEntity>();
    //return high-damaged target;
    public AliveEntity target
    { get { return (_targets.Count == 0) ? null : _targets.Values[_targets.Count - 1]; } }

    protected void Awake()
    {
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

    public void AddTarget(AliveEntity target)
    {
        if (isServer)
        {
            var rpc = new AddTarget(target.networkID);
            AddTarget(rpc);
            Send(rpc);
        }
    }

    public void AddTarget(AddTarget rpc)
    {
        var target = (AliveEntity)NetworkProgramUnity.currentInstance.signallersByID[rpc.targetID];
        if (!_targets.ContainsValue(target))
            _targets.Add(0, target);
    }

    public void RemoveTarget(AliveEntity target)
    {
        if (isServer)
            Send(new RemoveTarget(target.networkID));
    }

    public void RemoveTarget(RemoveTarget rpc)
    {
        var target = (AliveEntity)NetworkProgramUnity.currentInstance.signallersByID[rpc.targetID];
        if (_targets.ContainsValue(target))
            _targets.RemoveAt(_targets.IndexOfValue(target));
    }

    public void Chase()
    {
        _animator.SetBool("Chase", true);
    }

    public void ChaseStop()
    {
        _animator.SetBool("Chase", false);
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

    protected abstract List<Item> Items
    { get; }

    protected override void Die()
    {
        _animator.SetTrigger("Die");
    }

    public void DropItems()
    {
        if (isLocal)
            return;
        foreach (var item in Items)
            NetworkInstantiate(item.solidPrefab.GetComponent<NetworkScript>(), item, "OnDropItemInstantiate");
    }

    void SetActiveFalse()
    {
        if (isLocal)
            return;
        if (target)
            target.ExpUp(new ExpUp(10));
        InvokeRepeating("Respawn", 10.0f, float.MaxValue);
        Send(new SetActive(false));
    }

    public void Respawn()
    {
        CancelInvoke("Respawn");
        if (gameObject.activeSelf == false)
            Send(new SetActive(true));
        Send(new Reset(UnityEngine.Random.Range(0f, Mathf.PI * 2)));
    }

    public void Reset(Reset rpc)
    {
        transform.position = new Vector3(Mathf.Sin(rpc.positionSeed), 0, Mathf.Cos(rpc.positionSeed)) * UnityEngine.Random.Range(0f, spawner.spawnRange) + spawner.transform.position;
        transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0);
        _animator.Rebind();
        _targets.Clear();
    }

    protected override void OnDamaged(Damage damage)
    {
    }

    protected override void Knockdown()
    {
        _animator.SetTrigger("Knockdown");
    }

    public void OnDropItemInstantiate(ItemSolid item)
    {
        item.transform.position = transform.position + Vector3.up;
    }
}