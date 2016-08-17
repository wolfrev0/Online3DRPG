namespace TeraTaleNet
{
    public class Apple : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public sealed override string ingameName { get { return "사과"; } }
        public sealed override int price { get { return 5; } }
        public sealed override string effectExplanation { get { return "HP +15"; } }
        public sealed override string explanation { get { return "빨갛게 잘 익었다."; } }

        public Apple()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}