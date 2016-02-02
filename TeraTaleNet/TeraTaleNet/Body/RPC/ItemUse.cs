using UnityEngine;

namespace TeraTaleNet
{
    public class ItemUse : RPC
    {
        public int index;

        public ItemUse(int index)
            : base(RPCType.All)
        {
            this.index = index;
        }

        public ItemUse()
            : base(RPCType.All)
        { }
    }
}