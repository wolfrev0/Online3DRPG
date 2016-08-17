using UnityEngine;

namespace TeraTaleNet
{
    public class Axe : Weapon
    {
        public sealed override string ingameName { get { return "오래된 도끼"; } }
        public sealed override int price { get { return 50; } }
        public sealed override string effectExplanation { get { return "벌목"; } }
        public sealed override string explanation { get { return "벌목꾼의 손때가 느껴진다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.axe; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Axe()
        { }
    }
}