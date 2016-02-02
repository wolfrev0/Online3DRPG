using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Mineral : Enemy
{
    protected override List<Item> Items
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new Rock());
            else
                ret.Add(new IronOre());
            return ret;
        }
    }

    protected new void Start()
    {
        base.Start();
        usePeriodicSync = false;
    }

    protected override float CalculateDamage(float original, Weapon.Type weaponType)
    {
        if (weaponType != Weapon.Type.pickaxe)
        {
            if(isMine)
                FindObjectOfType<ChattingView>().PushGuideMessage("광물은 '곡괭이'로 채광할 수 있습니다.");
            return 0;
        }
        return original;
    }
}