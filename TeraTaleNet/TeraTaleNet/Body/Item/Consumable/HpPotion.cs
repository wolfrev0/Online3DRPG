namespace TeraTaleNet
{
    public class HpPotion : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public sealed override int price { get { return 20; } }
        public sealed override string effectExplanation { get { return "HP +50"; } }
        public sealed override string explanation { get { return "체력을 회복해주는 물약. \n그닥 맛있어 보이진 않다."; } }

        public HpPotion()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}