using UnityEngine;
using System.Collections;

public class MonsterSpawner : Entity
{
    public Enemy pfEnemy;
    public Enemy pfEnemy2;

    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")
        {
            NetworkInstantiate(pfEnemy.GetComponent<NetworkScript>());
            NetworkInstantiate(pfEnemy2.GetComponent<NetworkScript>());
        }
    }
}