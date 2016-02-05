namespace TeraTaleNet
{
    public class Bow : Weapon
    {
        public sealed override Type weaponType { get { return Type.bow; } }
        public override float bonusAttackDamage { get { return +5; } }
        public override float bonusAttackSpeed { get { return +0.5f; } }

        public Bow()
        { }
    }
}