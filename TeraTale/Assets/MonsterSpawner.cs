using UnityEngine;
using System.Collections;

public class MonsterSpawner : Entity
{
    public Enemy pfEnemy;
    
    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")
        {
            NetworkInstantiate(pfEnemy.GetComponent<NetworkScript>());
        }
    }
}