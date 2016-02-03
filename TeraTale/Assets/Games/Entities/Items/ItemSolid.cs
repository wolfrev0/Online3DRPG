using UnityEngine;
using TeraTaleNet;

public class ItemSolid : Entity
{
    public Item item;
    public ItemSpawnEffector _effector;

    public void OnNetInstantiate(Item item)
    {
        this.item = item;

        _effector = gameObject.AddComponent<ItemSpawnEffector>();
        var floater = gameObject.AddComponent<Floater>();
        floater.amplitude = 0.2f;
        floater.frequency = 2;
        floater.rotationSpeed = 1.5f;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (enabled == false)
            return;
        if (isServer && coll.tag == "Player")// Remove when the test ended.
        {
            var player = coll.GetComponent<Player>();
            player.AddItem(item);
            NetworkDestroy(this);
        }
    }
}