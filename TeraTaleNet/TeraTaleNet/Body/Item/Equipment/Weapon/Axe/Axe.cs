namespace TeraTaleNet
{
    public class Axe : Weapon
    {
        public sealed override Type weaponType { get { return Type.axe; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Axe()
        { }
    }
}