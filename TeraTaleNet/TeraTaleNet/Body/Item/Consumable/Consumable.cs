namespace TeraTaleNet
{
    public abstract class Consumable : Item
    {
        public sealed override int maxCount { get { return 100; } }

        public Consumable()
        { }
    }
}
