namespace TeraTaleNet
{
    public class Damage : RPC
    {
        public enum Type
        {
            Physical,
            Magic,
        }
        public Type type;
        public float amount;
        public float knockback;
        public bool knockdown;

        public Damage(Type type, float amount, float knockback, bool knockdown)
            : base(RPCType.Others)
        {
            this.type = type;
            this.amount = amount;
            this.knockback = knockback;
            this.knockdown = knockdown;
        }

        public Damage()
            : base(RPCType.Others)
        { }
    }
}