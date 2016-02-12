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

    public override float moveSpeed { get { return 2; } }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            ret.Add(new Bone());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 16; } }
}