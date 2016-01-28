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
            Item item = null;
            switch(itemName)
            {
                case "HpPotion":
                    item = new HpPotion();
                    break;
                case "Rock":
                    item = new Rock();
                    break;
                case "Dagger":
                    item = new Dagger();
                    break;
                case "Bow":
                    item = new Bow();
                    break;
                case "Wand":
                    item = new Wand();
                    break;
                case "Sword":
                    item = new Sword();
                    break;
            }
            NetworkInstantiate(item.solidPrefab.GetComponent<ItemSolid>(), item, "OnItemInstantiate");
        }
    }

    void OnItemInstantiate(ItemSolid itemSolid)
    {
        itemSolid.transform.position += Vector3.up * 2;
        itemSolid.transform.eulerAngles += new Vector3(0, 0, 45);
    }
}
