namespace TeraTaleNet
{
    public class Dagger : Weapon
    {
        public sealed override Type weaponType { get { return Type.knife; } }

        public Dagger()
        { }

        public Dagger(byte[] data)
            : base(data)
        { }
    }
}