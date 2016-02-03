namespace TeraTaleNet
{
    public class Apple : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public Apple()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}