namespace TeraTaleNet
{
    public class Dagger : Weapon
    {
        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +5; } }
        public override float bonusAttackSpeed { get { return +0.25f; } }

        public Dagger()
        { }
    }
}