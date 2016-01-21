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

    protected override List<Item> DropItems
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            else
                ret.Add(new Rock());
            return ret;
        }
    }
}