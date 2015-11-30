namespace TeraTaleNet
{
    public class PlayerJoin : Body
    {
        public string name;

        public PlayerJoin(string name)
        {
            this.name = name;
        }

        public PlayerJoin(byte[] data)
            : base(data)
        { }
    }
}