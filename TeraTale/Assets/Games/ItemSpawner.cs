using UnityEngine;
using TeraTaleNet;
using System;

public class ItemSpawner : NetworkScript
{
    public string itemName;

    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType("TeraTaleNet." + itemName + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            NetworkInstantiate(item.solidPrefab.GetComponent<ItemSolid>(), item, "OnItemInstantiate");
        }
    }

    void OnItemInstantiate(ItemSolid itemSolid)
    {
        itemSolid.transform.position += Vector3.up * 2;
        itemSolid.transform.eulerAngles += new Vector3(0, 0, 45);
    }
}
