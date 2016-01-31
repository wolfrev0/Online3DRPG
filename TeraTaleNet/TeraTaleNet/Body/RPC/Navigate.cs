using UnityEngine;

namespace TeraTaleNet
{
    public class Navigate : RPC
    {
        public Vector3 destination;

        public Navigate(Vector3 destination)
            : base(RPCType.All)
        {
            this.destination = destination;
        }

        public Navigate()
        { }
    }
}