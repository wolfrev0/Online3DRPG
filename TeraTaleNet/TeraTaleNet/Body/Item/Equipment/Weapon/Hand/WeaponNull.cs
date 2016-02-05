namespace TeraTaleNet
{
    public class WeaponNull : Weapon
    {
        public sealed override Type weaponType { get { return Type.hand; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public WeaponNull()
        { }
    }
}