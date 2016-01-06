namespace TeraTaleNet
{
    public class SwitchWorld : Body
    {
        public string user;
        public string world;

        public SwitchWorld(string user, string world)
        {
            this.user = user;
            this.world = world;
        }

        public SwitchWorld(byte[] data)
            : base(data)
        { }
    }
}