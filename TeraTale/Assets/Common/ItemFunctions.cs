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
        RecallScroll.onUse += (Item item, object target) =>
        {
            if (isServer)
            {
                var player = (Player)target;
                player.SwitchWorld("Town");
            }
        };
        Equipment.onUse += (Item item, object target) =>
        {
            var player = (Player)target;
            var equipment = (Equipment)item;
            if (player.IsEquiping(equipment))
            {
                switch (equipment.equipmentType)
                {
                    case Equipment.Type.Accessory:
                        player.Equip(new AccessoryNull());
                        break;
                    case Equipment.Type.Coat:
                        break;
                    case Equipment.Type.Gloves:
                        break;
                    case Equipment.Type.Hat:
                        break;
                    case Equipment.Type.Pants:
                        break;
                    case Equipment.Type.Shoes:
                        break;
                    case Equipment.Type.Weapon:
                        player.Equip(new WeaponNull());
                        break;
                }
            }
            else
                player.Equip((Equipment)item);
        };
        Scroll.onUse += (Item item, object target) =>
        {
            ScrollIngredientView.instance.Open((Scroll)item);
        };
    }
}