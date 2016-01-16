namespace TeraTaleNet
{
    public class Chase : RPC
    {
        public int targetSignallerID = 0;

        public Chase(int targetSignallerID)
            : base(RPCType.All)
        {
            this.targetSignallerID = targetSignallerID;
        }

        public Chase(byte[] data)
            : base(data)
        { }
    }
}