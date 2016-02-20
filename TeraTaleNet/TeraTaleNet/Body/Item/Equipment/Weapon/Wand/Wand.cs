using UnityEngine;

namespace TeraTaleNet
{
    public class Wand : Weapon
    {
        public sealed override string ingameName { get { return "방출의 마법봉"; } }
        public sealed override int price { get { return 80; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "누군가 실험하다 버린 실패작. 그럭저럭 쓸만하다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.wand; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Wand()
        { }
    }
}