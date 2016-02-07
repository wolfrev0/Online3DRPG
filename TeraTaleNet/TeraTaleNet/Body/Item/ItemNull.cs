namespace TeraTaleNet
{
    public class ItemNull : Item
    {
        public sealed override Type itemType { get { return Type.none; } }
        public sealed override int price { get { return 0; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return ""; } }

        public sealed override int maxCount { get { return int.MaxValue; } }

        public ItemNull()
        { }
    }
}