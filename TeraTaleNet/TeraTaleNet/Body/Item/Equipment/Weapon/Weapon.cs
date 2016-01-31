namespace TeraTaleNet
{
    public abstract class Weapon : Equipment
    {
        public new enum Type
        {
            hand,
            knife,
            bow,
            wand,
            gun,
        }

        public sealed override Equipment.Type equipmentType { get { return Equipment.Type.Weapon; } }
        public abstract Type weaponType { get; }

        public Weapon()
        { }
    }
}