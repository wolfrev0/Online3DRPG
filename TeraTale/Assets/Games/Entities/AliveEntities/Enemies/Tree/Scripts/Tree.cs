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

    protected new void Start()
    {
        base.Start();
        usePeriodicSync = false;
    }

    protected override float CalculateDamage(Damage damage)
    {
        if (damage.weaponType != Weapon.Type.axe)
        {
            if (damage.sendedUser == userName)
                FindObjectOfType<ChattingView>().PushGuideMessage("나무는 '도끼'로 벌목할 수 있습니다.");
            return 0;
        }
        return damage.amount;
    }
}