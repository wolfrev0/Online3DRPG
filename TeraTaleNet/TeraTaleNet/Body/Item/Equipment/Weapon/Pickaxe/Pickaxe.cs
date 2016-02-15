using UnityEngine;

namespace TeraTaleNet
{
    public class Pickaxe : Weapon
    {
        public sealed override int price { get { return 80; } }
        public sealed override string effectExplanation { get { return "채광"; } }
        public sealed override string explanation { get { return "광부의 손때가 느껴진다. 녹슬어서 버린듯하다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.pickaxe; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public Pickaxe()
        { }
    }
}