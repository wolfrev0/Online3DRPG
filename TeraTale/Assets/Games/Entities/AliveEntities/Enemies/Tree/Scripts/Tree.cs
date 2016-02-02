using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Tree : Enemy
{
    protected override List<Item> Items
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new Apple());
            else
                ret.Add(new Log());
            return ret;
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        usePeriodicSync = false;
        transform.rotation = Quaternion.identity;
        GetComponent<Animator>().Rebind();
    }
}