using UnityEngine;

namespace TeraTaleNet
{
    public class Reset : RPC
    {
        public float positionSeed;
        public float lengthSeed;

        public Reset(float positionSeed, float lengthSeed)
            : base(RPCType.All)
        {
            this.positionSeed = positionSeed;
            this.lengthSeed = lengthSeed;
        }

        public Reset()
            : base(RPCType.All)
        { }
    }
}