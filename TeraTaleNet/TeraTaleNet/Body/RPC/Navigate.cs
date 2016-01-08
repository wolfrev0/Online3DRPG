namespace TeraTaleNet
{
    public class Navigate : RPC
    {
        public float x, y, z;

        public Navigate(float x, float y, float z)
            : base(RPCType.All)
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