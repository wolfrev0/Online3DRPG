namespace TeraTaleNet
{
    public class ExpUp : RPC
    {
        public float amount;

        public ExpUp(float amount)
            : base(RPCType.Others)
        {
            this.amount = amount;
        }

        public ExpUp(byte[] data)
                : base(data)
        { }
    }
}