namespace TeraTaleNet
{
    public class HpPotion : Consumable
    {
        public delegate void OnUse(Item item);
        static public OnUse onUse;

        public HpPotion()
        { }

        public HpPotion(byte[] data)
            : base(data)
        { }

        public override void Use()
        {
            onUse(this);
        }
    }
}