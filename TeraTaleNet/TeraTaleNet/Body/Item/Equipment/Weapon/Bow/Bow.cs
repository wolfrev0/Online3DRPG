using UnityEngine;

namespace TeraTaleNet
{
    public class Bow : Weapon
    {
        public sealed override string ingameName { get { return "고무줄 활"; } }
        public sealed override int price { get { return 70; } }
        public sealed override string effectExplanation { get { return "공격력 +5"; } }
        public sealed override string explanation { get { return "활시위가 고무줄로 만들어져있다. 의외로 장난감일지도."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.LeftHand; } }

        public sealed override Type weaponType { get { return Type.bow; } }
        public override float bonusAttackDamage { get { return +5; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Bow()
        { }
    }
}