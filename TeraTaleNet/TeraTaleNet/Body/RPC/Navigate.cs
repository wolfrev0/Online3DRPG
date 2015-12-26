namespace TeraTaleNet
{
    public class Navigate : RPC
    {
        public float x, y, z;

        public Navigate(RPCType rpcType, float x, float y, float z)
            : base(rpcType)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Navigate(byte[] data)
            : base(data)
        { }
    }
}