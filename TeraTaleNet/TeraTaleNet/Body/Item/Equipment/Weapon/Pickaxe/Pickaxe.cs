namespace TeraTaleNet
{
    public class Pickaxe : Weapon
    {
        public sealed override Type weaponType { get { return Type.pickaxe; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Pickaxe()
        { }
    }
}