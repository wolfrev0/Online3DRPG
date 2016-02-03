using UnityEngine;
using TeraTaleNet;

public class ItemFunctions : NetworkScript
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        HpPotion.onUse += (Item item, object target) =>
        {
            if (isServer)
            {
                var player = (Player)target;
                player.Heal(new Heal("", 30));
            }
        };
        Apple.onUse += (Item item, object target) =>
        {
            if (isServer)
            {
                var player = (Player)target;
                player.Heal(new Heal("", 10));
            }
        };
        Equipment.onUse += (Item item, object target) =>
        {
            var player = (Player)target;
            if (player.IsEquiping((Equipment)item))
                player.Equip(new WeaponNull());
            else
                player.Equip((Equipment)item);
        };
    }
}