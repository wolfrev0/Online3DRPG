using UnityEngine;

namespace TeraTaleNet
{
    public class WeaponNull : Weapon
    {
        public sealed override string ingameName { get { return ""; } }
        public sealed override int price { get { return 0; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return ""; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.hand; } }
        public override float bonusAttackDamage { get { return +0; } }
        public override float bonusAttackSpeed { get { return +0; } }

        public WeaponNull()
        { }
    }
}