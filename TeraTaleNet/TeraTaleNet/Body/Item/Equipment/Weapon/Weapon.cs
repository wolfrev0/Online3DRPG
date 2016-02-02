namespace TeraTaleNet
{
    public abstract class Weapon : Equipment
    {
        public new enum Type
        {
            none,
            hand,
            knife,
            bow,
            wand,
            axe,
            pickaxe,
        }

        public sealed override Equipment.Type equipmentType { get { return Equipment.Type.Weapon; } }
        public abstract Type weaponType { get; }

        public Weapon()
        { }
    }
}