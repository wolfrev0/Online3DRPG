using UnityEngine;

namespace TeraTaleNet
{
    public class Respawn : RPC
    {
        public float positionSeed;
        public float lengthSeed;

        public Respawn(float positionSeed, float lengthSeed)
            : base(RPCType.All)
        {
            this.positionSeed = positionSeed;
            this.lengthSeed = lengthSeed;
        }

        public Respawn()
            : base(RPCType.All)
        { }
    }
}