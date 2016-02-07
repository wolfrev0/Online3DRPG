namespace TeraTaleNet
{
    public class Apple : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public sealed override int price { get { return 5; } }
        public sealed override string effectExplanation { get { return "HP 15 회복"; } }
        public sealed override string explanation { get { return "빨갛게 잘 익은 사과. 맛이 좋다."; } }

        public Apple()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}