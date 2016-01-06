using UnityEngine;

public class Portal : MonoBehaviour
{
    public string targetWorld;

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
            if (player.isMine)
                player.SwitchWorld(targetWorld);
        }
    }
}