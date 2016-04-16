namespace TeraTaleNet
{
    public class PlayerDisconnect : Body
    {
        public string name;

        public PlayerDisconnect(string name)
        {
            this.name = name;
        }

        public PlayerDisconnect()
        { }
    }
}