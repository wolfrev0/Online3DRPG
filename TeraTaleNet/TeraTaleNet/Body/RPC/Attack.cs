namespace TeraTaleNet
{
    public class Attack : RPC
    {
        public Attack(RPCType rpcType)
            : base(rpcType)
        { }

        public Attack(byte[] data)
            : base(data)
        { }
    }
}