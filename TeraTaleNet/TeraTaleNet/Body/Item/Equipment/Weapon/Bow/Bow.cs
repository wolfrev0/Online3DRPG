namespace TeraTaleNet
{
    public class Bow : Weapon
    {
        public sealed override Type weaponType { get { return Type.bow; } }

        public Bow()
        { }

        public Bow(byte[] data)
            : base(data)
        { }
    }
}