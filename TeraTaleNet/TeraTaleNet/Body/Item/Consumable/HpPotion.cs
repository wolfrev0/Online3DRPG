namespace TeraTaleNet
{
    public class HpPotion : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public HpPotion()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}