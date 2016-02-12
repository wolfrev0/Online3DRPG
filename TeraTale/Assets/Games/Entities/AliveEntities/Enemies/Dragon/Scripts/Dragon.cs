using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Dragon : Enemy
{
    public Projectile pfMeteor;
    public FireBreath fireBreath;

    Collider _ardColl;
    int nextAttackType;
    System.Random random = new System.Random();
    
    public override float moveSpeed { get { return 0; } }

    public override void OnAttackAnimationEnd(Collider ardColl)
    {
        _ardColl = ardColl;
        if (isServer)
            Send(new SetDragonNextAttack(Random.Range(2f, 8f), Random.Range(0, 2), Random.Range(int.MinValue, int.MaxValue)));
    }

    public void SetDragonNextAttack(SetDragonNextAttack rpc)
    {
        Invoke("ARDCollEnable", rpc.nextEnableDelay);
        nextAttackType = rpc.attackType;
        random = new System.Random(rpc.seed);
    }

    void ARDCollEnable()
    {
        _ardColl.enabled = true;
    }

    public override void Attack()
    {
        base.Attack();
        _animator.SetInteger("AttackType", nextAttackType);
    }

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    void MeteorStart()
    {
        for (int i = 0; i < 10; i++)
            StartCoroutine(CastMeteor(null));
        foreach (var target in targets)
            StartCoroutine(CastMeteor(target));
    }

    IEnumerator CastMeteor(AliveEntity target)
    {
        yield return new WaitForSeconds((float)random.NextDouble());

        var meteor = Instantiate(pfMeteor);
        var xzSeed = (float)random.NextDouble() * 2 * Mathf.PI;
        if (target == null)
            meteor.transform.position = transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * (float)random.NextDouble() * 10 + Vector3.up * 50;
        else
            meteor.transform.position = target.transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * (float)random.NextDouble() + Vector3.up * 50;
        meteor.direction = new Vector3(0, -1, 0);
    }

    void FireBreathBegin()
    {
        fireBreath.On();
    }

    void FireBreathEnd()
    {
        fireBreath.Off();
    }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            ret.Add(new Apple());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 10; } }
}