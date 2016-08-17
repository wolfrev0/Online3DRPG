using UnityEngine;

namespace TeraTaleNet
{
    public class AccessoryNull : Accessory
    {
        public sealed override string ingameName { get { return ""; } }
        public sealed override int price { get { return 0; } }
        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return ""; } }
        public sealed override HumanBodyBones targetBone { get { return HumanBodyBones.Hips; } }
        public sealed override float bonusMoveSpeed { get { return 0; } }

        public AccessoryNull()
        { }
    }
}