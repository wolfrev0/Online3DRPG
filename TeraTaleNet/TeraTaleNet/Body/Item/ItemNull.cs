namespace TeraTaleNet
{
    public class ItemNull : Item
    {
        public sealed override int maxCount { get { return int.MaxValue; } }
        public sealed override bool isNull { get { return true; } }

        public ItemNull()
        { }
    }
}