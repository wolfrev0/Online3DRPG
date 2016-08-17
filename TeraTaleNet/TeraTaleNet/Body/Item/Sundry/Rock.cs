namespace TeraTaleNet
{
    public class Rock : Sundry
    {
        public sealed override int price { get { return 5; } }
        public sealed override string ingameName { get { return "바위"; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "주변에서 흔히 볼 수 있는 바위다."; } }

        public Rock()
        { }
    }
}