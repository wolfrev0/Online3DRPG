namespace TeraTaleNet
{
    public class Bone : Sundry
    {
        public sealed override string ingameName { get { return "뼈"; } }
        public sealed override int price { get { return 10; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "스켈레톤의 혼이 서려있다."; } }

        public Bone()
        { }
    }
}