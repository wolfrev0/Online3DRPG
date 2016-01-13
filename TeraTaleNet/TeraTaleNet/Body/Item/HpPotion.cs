namespace TeraTaleNet
{
    public class HpPotion : Item
    {
        public delegate void OnUse();
        static public OnUse onUse;

        public HpPotion()
        { }

        public HpPotion(byte[] data)
            : base(data)
        { }

        public override void Use()
        {
            onUse();
        }
    }
}