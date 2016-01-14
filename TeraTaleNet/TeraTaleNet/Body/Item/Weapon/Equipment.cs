namespace TeraTaleNet
{
    public abstract class Equipment : Item
    {
        public enum Type
        {
            Weapon,
            Coat,
            Pants,
            Shoes,
            Gloves,
            Hat,
        }

        public delegate void OnUse(Item item);
        static public OnUse onUse;

        public abstract Type type { get; }

        public sealed override int maxCount { get { return 1; } }

        public Equipment()
        { }

        public Equipment(byte[] data)
            : base(data)
        { }

        public override void Use()
        {
            onUse(this);
        }
    }
}
