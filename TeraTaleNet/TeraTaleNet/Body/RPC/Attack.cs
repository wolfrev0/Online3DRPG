namespace TeraTaleNet
{
    public class Attack : RPC
    {
        public Attack()
            : base(RPCType.All)
        { }

        public Attack(byte[] data)
            : base(data)
        { }
    }
}