using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : Enemy
{
    public Projectile pfMeteor;
    public Projectile pfWind;
    FireBreath fireBreath;
    Image bossHpBar;

    Collider _ardColl;
    int nextAttackType;
    System.Random random = new System.Random();

    public override float respawnDelay { get { return 300; } }
    public override float baseMoveSpeed { get { return 0; } }
    public override float hideTime { get { return 60; } }

    public override void OnAttackAnimationEnd(Collider ardColl)
    {
        _ardColl = ardColl;
        if (isServer)
            Send(new SetDragonNextAttack(Random.Range(2f, 8f), Random.Range(0, 3), Random.Range(int.MinValue, int.MaxValue)));
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
        fireBreath = GetComponentInChildren<FireBreath>();
        GetComponentInChildren<AttackSubjectImpl>().damageCalculator = original => original * 0.1f;
    }

    new void Start()
    {
        base.Start();
        bossHpBar = GameObject.Find("BossHp").GetComponent<Image>();
        BossMessage.instance.Show();
    }

    protected new void Update()
    {
        base.Update();
        bossHpBar.fillAmount = hp / hpMax;
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
        meteor.GetComponent<AttackSubject>().owner = this;
    }

    void FireBreathBegin()
    {
        fireBreath.On();
    }

    void FireBreathEnd()
    {
        fireBreath.Off();
    }

    void Wind()
    {
        if (mainTarget)
        {
            var wind = Instantiate(pfWind);
            var xzSeed = (float)random.NextDouble() * 2 * Mathf.PI;
            var destination = mainTarget.transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * (float)random.NextDouble() * 3;
            wind.transform.position = transform.position + Vector3.up * 3 - transform.right * 3;
            wind.direction = (destination - wind.transform.position).normalized;
            wind.GetComponent<AttackSubject>().owner = this;

            wind = Instantiate(pfWind);
            xzSeed = (float)random.NextDouble() * 2 * Mathf.PI;
            destination = mainTarget.transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * (float)random.NextDouble() * 3;
            wind.transform.position = transform.position + Vector3.up * 3 + transform.right * 3;
            wind.direction = (destination - wind.transform.position).normalized;
            wind.GetComponent<AttackSubject>().owner = this;
        }
    }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            ret.Add(new HpPotion());
            ret.Add(new HpPotion());
            ret.Add(new HpPotion());
            ret.Add(new Apple());
            ret.Add(new Apple());
            ret.Add(new Bone());
            ret.Add(new Bone());
            ret.Add(new Bone());
            ret.Add(new LapisLazuliOre());
            ret.Add(new DevilSheen());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 200; } }

    protected override void Die()
    {
        base.Die();
        var exit = FindObjectOfType<Portal>();
        var pos = exit.transform.position;
        pos.y = 0.1f;
        exit.transform.position = pos;
        BossClearMessage.instance.Invoke("Show", 4);
    }
}