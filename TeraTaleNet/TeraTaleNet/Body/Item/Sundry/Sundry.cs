namespace TeraTaleNet
{
    public abstract class Sundry : Item
    {
        public sealed override Type itemType { get { return Type.sundry; } }
        public sealed override int maxCount { get { return 100; } }

        public Sundry()
        { }
    }
}
