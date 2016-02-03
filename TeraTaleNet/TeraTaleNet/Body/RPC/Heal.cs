namespace TeraTaleNet
{
    public class Heal : RPC
    {
        public string from;
        public float amount;

        public Heal(string from, float amount)
            : base(RPCType.Others)
        {
            this.from = from;
            this.amount = amount;
        }

        public Heal()
            : base(RPCType.Others)
        { }
    }
}