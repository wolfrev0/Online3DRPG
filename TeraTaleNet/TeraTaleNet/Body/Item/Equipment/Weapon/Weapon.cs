namespace TeraTaleNet
{
    public class Weapon : Equipment
    {
        public sealed override Type type { get { return Type.Weapon; } }

        public Weapon()
        { }

        public Weapon(byte[] data)
            : base(data)
        { }
    }
}