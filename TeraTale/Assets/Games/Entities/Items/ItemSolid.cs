using UnityEngine;
using TeraTaleNet;

public class ItemSolid : Entity
{
    public Item item;
    public ItemSpawnEffector _effector;

    protected new void OnEnable()
    {
        //base.OnEnable();
    }

    public void OnNetInstantiate(ItemSolidArgument arg)
    {
        item = arg.item;

        transform.position = arg.spawnPos;

        _effector = gameObject.AddComponent<ItemSpawnEffector>();
        _effector.xzAngle = arg.xzAngle;
        _effector.xzSpeed = arg.xzSpeed;
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
            if (player.CanAddItem(item, 1))
            {
                player.AddItem(item);
                NetworkDestroy(this);
            }
        }
    }
}