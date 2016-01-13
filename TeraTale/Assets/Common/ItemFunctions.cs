using UnityEngine;
using TeraTaleNet;

public class ItemFunctions : NetworkScript
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        HpPotion.onUse += () => 
        {
            var player = Player.FindPlayerByName(userName);
        };
    }
}
