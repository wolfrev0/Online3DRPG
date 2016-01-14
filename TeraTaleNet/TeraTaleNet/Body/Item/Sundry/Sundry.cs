namespace TeraTaleNet
{
    public abstract class Sundry : Item
    {
        public sealed override int maxCount { get { return 100; } }

        public Sundry()
        { }

        public Sundry(byte[] data)
            : base(data)
        { }
    }
}
