namespace TeraTaleNet
{
    public class Wand : Weapon
    {
        public sealed override Type weaponType { get { return Type.wand; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Wand()
        { }
    }
}