namespace TeraTaleNet
{
    public class Wand : Weapon
    {
        public sealed override Type weaponType { get { return Type.wand; } }

        public Wand()
        { }

        public Wand(byte[] data)
            : base(data)
        { }
    }
}