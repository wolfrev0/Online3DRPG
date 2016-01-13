using UnityEngine;
using TeraTaleNet;

public class ItemSolid : Entity
{
    Item _item;

    public void OnNetInstantiate(Item item)
    {
        _item = item;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")// Remove when the test ended.
        {
            var player = coll.GetComponent<Player>();
            player.AddItem(_item);
            NetworkDestroy(this);
        }
    }
}