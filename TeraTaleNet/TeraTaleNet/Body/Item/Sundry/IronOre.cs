namespace TeraTaleNet
{
    public class IronOre : Sundry
    {
        public sealed override int price { get { return 15; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "여러모로 쓸모가 많은듯하다."; } }

        public IronOre()
        { }
    }
}