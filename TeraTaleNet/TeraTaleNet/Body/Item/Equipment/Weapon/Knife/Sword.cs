namespace TeraTaleNet
{
    public class Sword : Weapon
    {
        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +6; } }
        public override float bonusAttackSpeed { get { return +0.3f; } }

        public Sword()
        { }
    }
}