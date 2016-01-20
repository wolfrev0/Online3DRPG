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
        public bool fallDown;

        public Damage(Type type, float amount, float knockback, bool fallDown)
            : base(RPCType.Others)
        {
            this.type = type;
            this.amount = amount;
            this.knockback = knockback;
            this.fallDown = fallDown;
        }

        public Damage(byte[] data)
                : base(data)
        { }
    }
}