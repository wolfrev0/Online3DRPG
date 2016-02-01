namespace TeraTaleNet
{
    public class WeaponNull : Weapon
    {
        public sealed override Type weaponType { get { return Type.hand; } }

        public WeaponNull()
        { }
    }
}