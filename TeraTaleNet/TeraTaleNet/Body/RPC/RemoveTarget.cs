namespace TeraTaleNet
{
    public class RemoveTarget : RPC
    {
        public int targetID;

        public RemoveTarget(int targetID)
            : base(RPCType.All)
        {
            this.targetID = targetID;
        }

        public RemoveTarget()
            : base(RPCType.All)
        { }
    }
}