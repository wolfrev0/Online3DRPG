using UnityEngine;

namespace TeraTaleNet
{
    public class FoxTail : Accessory
    {
        public sealed override string ingameName { get { return "여우 꼬리"; } }
        public sealed override int price { get { return 100; } }
        public sealed override string effectExplanation { get { return "이동속도 +1.5"; } }
        public sealed override string explanation { get { return "벨로스 숲에 사는 여우의 꼬리. 매우 부드러운 최상품이다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.Hips; } }
        public sealed override float bonusMoveSpeed { get { return 1.5f; } }

        public FoxTail()
        { }
    }
}