using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Skeleton : Enemy
{
    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    public override float respawnDelay { get { return 60; } }
    public override float baseMoveSpeed { get { return 2; } }

    protected override void OnDamaged(Damage damage)
    {
        base.OnDamaged(damage);
        GlobalSound.instance.PlaySkeletonHit();
    }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            ret.Add(new Bone());
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            if (Random.Range(0, 4) == 0)
                ret.Add(new BowScroll());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 16; } }
}