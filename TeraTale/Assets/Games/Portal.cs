using UnityEngine;

public class Portal : Entity
{
    public string targetWorld;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
            var player = coll.GetComponent<Player>();
            player.SwitchWorld(targetWorld);
        }
    }
}