using UnityEngine;
using TeraTaleNet;
using System;

public class ItemSpawner : Entity
{
    public string itemName;

    void OnTriggerEnter(Collider coll)
    {
        if(isServer&&coll.tag == "Player")
        {
            Item item;
            if (itemName == "HpPotion")
                item = new HpPotion();
            else if(itemName == "Rock")
                item = new Rock();
            else
                item = new Dagger();
            NetworkInstantiate(item.solidPrefab.GetComponent<ItemSolid>(), item);
        }
    }
}
