using UnityEngine;

namespace TeraTaleNet
{
    public class DevilSheen : Weapon
    {
        public sealed override string ingameName { get { return "하제 소드"; } }
        public sealed override int price { get { return 999; } }
        public sealed override string effectExplanation { get { return "공격력 +20\n공격속도 +0.5"; } }
        public sealed override string explanation { get { return "사랑스러운 외모와 달리 매우 강력하다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +20; } }
        public override float bonusAttackSpeed { get { return +0.5f; } }

        public DevilSheen()
        { }
    }
}