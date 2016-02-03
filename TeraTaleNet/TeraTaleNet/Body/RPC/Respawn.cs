using UnityEngine;

namespace TeraTaleNet
{
    public class Reset : RPC
    {
        public float positionSeed;

        public Reset(float positionSeed)
            : base(RPCType.All)
        {
            this.positionSeed = positionSeed;
        }

        public Reset()
            : base(RPCType.All)
        { }
    }
}