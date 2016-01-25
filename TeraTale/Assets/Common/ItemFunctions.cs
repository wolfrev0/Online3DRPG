using UnityEngine;
using TeraTaleNet;

public class ItemFunctions : NetworkScript
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        HpPotion.onUse += (Item item) =>
        {
            var player = Player.mine;
            player.Send(new Heal("", 30));
        };
        Equipment.onUse += (Item item) =>
        {
            var player = Player.mine;
            if (player.IsEquiping((Equipment)item))
                player.Equip(new WeaponNull());
            else
                player.Equip((Equipment)item);

        };
    }
}
