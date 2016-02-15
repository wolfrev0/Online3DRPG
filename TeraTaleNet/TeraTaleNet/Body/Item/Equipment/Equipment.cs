namespace TeraTaleNet
{
    public abstract class Equipment : Item
    {
        public new enum Type
        {
            Weapon,
            Coat,
            Pants,
            Shoes,
            Gloves,
            Hat,
            Accessory,
        }

        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public abstract Type equipmentType { get; }

        public sealed override Item.Type itemType { get { return Item.Type.equipment; } }
        public sealed override int maxCount { get { return 1; } }

        public Equipment()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}
