using UnityEngine;

namespace TeraTaleNet
{
    public class Dagger : Weapon
    {
        public sealed override string ingameName { get { return "녹슨 단검"; } }
        public sealed override int price { get { return 80; } }
        public sealed override string effectExplanation { get { return "공격력 +5\n공격속도 +0.3"; } }
        public sealed override string explanation { get { return "칼날이 녹슬어 있다. 짧지만 날렵해서 유용하다."; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.RightHand; } }

        public sealed override Type weaponType { get { return Type.knife; } }
        public override float bonusAttackDamage { get { return +5; } }
        public override float bonusAttackSpeed { get { return +0.3f; } }

        public Dagger()
        { }
    }
}