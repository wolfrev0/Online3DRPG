using UnityEngine;

namespace TeraTaleNet
{
    public class Sword : Weapon
    {
        public sealed override int price { get { return 80; } }
        public sealed override string effectExplanation { get { return "공격력 +8"; } }
        public sealed override string explanation { get { return "오래되어 이곳 저곳 녹슬어있지만, 그럭저럭 쓸만해 보인다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +8; } }
        public override float bonusAttackSpeed { get { return +0f; } }

        public Sword()
        { }
    }
}