namespace TeraTaleNet
{
    public class RemoveBufferedRPC : Body
    {
        public string caller;
        public string typeName;
        public int networkID;

        public RemoveBufferedRPC(string caller, string typeName, int networkID)
        {
            this.caller = caller;
            this.typeName = typeName;
            this.networkID = networkID;
        }

        public RemoveBufferedRPC(byte[] data)
            : base(data)
        { }
    }
}