using UnityEngine;

public class Portal : MonoBehaviour
{
    string targetWorld;

    void Awake()
    {

    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
            var player = coll.GetComponent<Player>();
            player.SwitchWorld(targetWorld);
        }
    }
}