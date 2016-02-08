using UnityEngine;
using TeraTaleNet;

public class ItemFunctions : NetworkScript
{
    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        HpPotion.onUse += (Item item, object target) =>
        {
            if (isServer)
            {
                var player = (Player)target;
                player.Heal(new Heal("", 50));
            }
        };
        Apple.onUse += (Item item, object target) =>
        {
            if (isServer)
            {
                var player = (Player)target;
                player.Heal(new Heal("", 15));
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
        Scroll.onUse += (Item item, object target) =>
        {
            ScrollIngredientView.instance.Open((Scroll)item);
        };
    }
}