namespace TeraTaleNet
{
    public class NetworkInstantiateRequest : Body
    {
        public string owner;
        public int prefabIndex;

        public NetworkInstantiateRequest(string owner, int prefabIndex)
        {
            this.owner = owner;
            this.prefabIndex = prefabIndex;
        }

        public NetworkInstantiateRequest(byte[] data)
            : base(data)
        { }
    }
}