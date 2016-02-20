using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Demon : Enemy
{
    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    public override float baseMoveSpeed { get { return 3; } }

    protected override void OnDamaged(Damage damage)
    {
        base.OnDamaged(damage);
        GlobalSound.instance.PlayZombieHit();
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