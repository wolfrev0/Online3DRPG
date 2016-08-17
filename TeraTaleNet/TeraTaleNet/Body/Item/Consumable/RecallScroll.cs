namespace TeraTaleNet
{
    public class RecallScroll : Consumable
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public sealed override string ingameName { get { return "귀환서"; } }
        public sealed override int price { get { return 35; } }
        public sealed override string effectExplanation { get { return "마을로 즉시 귀환합니다."; } }
        public sealed override string explanation { get { return "더 나아가기 위한 후퇴를 부끄러워할 필요가 없지. -Nox-"; } }

        public RecallScroll()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}