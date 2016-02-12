namespace TeraTaleNet
{
    public class SetDragonNextAttack : RPC
    {
        public float nextEnableDelay;
        public int attackType;
        public int seed;

        public SetDragonNextAttack(float nextEnableDelay, int attackType, int seed)
            : base(RPCType.All)
        {
            this.nextEnableDelay = nextEnableDelay;
            this.attackType = attackType;
            this.seed = seed;
        }

        public SetDragonNextAttack()
            : base(RPCType.All)
        { }
    }
}