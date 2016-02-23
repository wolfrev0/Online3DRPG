using UnityEngine;

namespace TeraTaleNet
{
    public class Sheen : Weapon
    {
        public sealed override string ingameName { get { return "광휘의 검"; } }
        public sealed override int price { get { return 999; } }
        public sealed override string effectExplanation { get { return "공격력 +50\n공격속도 -0.3"; } }
        public sealed override string explanation { get { return "소화기로 맞고 싶냐? -광휘쌤-"; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +50; } }
        public override float bonusAttackSpeed { get { return -0.3f; } }

        public Sheen()
        { }
    }
}