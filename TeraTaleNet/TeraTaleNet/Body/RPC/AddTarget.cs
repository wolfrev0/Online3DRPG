namespace TeraTaleNet
{
    public class AddTarget : RPC
    {
        public int targetID;

        public AddTarget(int targetID)
            : base(RPCType.Others)
        {
            this.targetID = targetID;
        }

        public AddTarget()
            : base(RPCType.Others)
        { }
    }
}