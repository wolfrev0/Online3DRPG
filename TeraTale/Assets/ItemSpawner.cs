using UnityEngine;

public class ItemSpawner : Entity
{
    public NetworkScript pfItem;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
            if(isServer)
                NetworkInstantiate(pfItem);
        }
    }
}
