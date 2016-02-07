namespace TeraTaleNet
{
    public abstract class Consumable : Item
    {
        public sealed override Type itemType { get { return Type.consumable; } }
        public sealed override int maxCount { get { return 100; } }

        public Consumable()
        { }
    }
}
