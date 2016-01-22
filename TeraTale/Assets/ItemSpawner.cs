using UnityEngine;
using TeraTaleNet;
using System;

public class ItemSpawner : Entity
{
    public string itemName;

    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")
        {
            Item item;
            if (itemName == "HpPotion")
                item = new HpPotion();
            else if (itemName == "Rock")
                item = new Rock();
            else if (itemName == "Dagger")
                item = new Dagger();
            else if (itemName == "Bow")
                item = new Bow();
            else
                item = new Sword();
            NetworkInstantiate(item.solidPrefab.GetComponent<ItemSolid>(), item);
        }
    }
}
