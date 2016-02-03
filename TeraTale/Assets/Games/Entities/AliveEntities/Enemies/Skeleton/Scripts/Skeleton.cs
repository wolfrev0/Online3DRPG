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

    protected override List<Item> Items
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new Bone());
            else
                ret.Add(new HpPotion());
            return ret;
        }
    }
}