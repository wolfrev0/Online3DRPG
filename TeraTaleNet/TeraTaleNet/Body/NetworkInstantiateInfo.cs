namespace TeraTaleNet
{
    public class NetworkInstantiateInfo : Body
    {
        public string owner;
        public int prefabIndex;
        public int signallerID;

        public NetworkInstantiateInfo(string owner, int prefabIndex, int signallerID)
        {
            this.owner = owner;
            this.prefabIndex = prefabIndex;
            this.signallerID = signallerID;
        }

        public NetworkInstantiateInfo(byte[] data)
            : base(data)
        { }
    }
}