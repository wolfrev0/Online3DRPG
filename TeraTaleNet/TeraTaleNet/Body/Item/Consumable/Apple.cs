namespace TeraTaleNet
{
    public class Apple : Consumable
    {
        public delegate void OnUse(Item item);
        static public OnUse onUse;

        public Apple()
        { }

        public override void Use()
        {
            onUse(this);
        }
    }
}