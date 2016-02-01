namespace TeraTaleNet
{
    public class Heal : RPC
    {
        public string healer;
        public float amount;

        public Heal(string healer, float amount)
            : base(RPCType.Others)
        {
            this.healer = healer;
            this.amount = amount;
        }

        public Heal()
            : base(RPCType.Others)
        { }
    }
}