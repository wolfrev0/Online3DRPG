namespace TeraTaleNet
{
    public class SwitchWorld : RPC
    {
        public string user;
        public string world;

        public SwitchWorld(RPCType type, string receiver, string user, string world)
            :base(type, receiver)
        {
            this.user = user;
            this.world = world;
        }

        public SwitchWorld()
        { }
    }
}