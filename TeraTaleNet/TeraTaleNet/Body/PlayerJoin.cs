namespace TeraTaleNet
{
    public class PlayerJoin : Body
    {
        public string nickName;
        public string world;

        public PlayerJoin(string nickName, string world)
        {
            this.nickName = nickName;
            this.world = world;
        }

        public PlayerJoin(byte[] data)
            : base(data)
        { }
    }
}