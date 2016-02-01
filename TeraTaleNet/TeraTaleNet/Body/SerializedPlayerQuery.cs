namespace TeraTaleNet
{
    public class SerializedPlayerQuery : Body
    {
        public string sender;
        public string player;
        
        public SerializedPlayerQuery(string sender, string player)
        {
            this.sender = sender;
            this.player = player;
        }

        public SerializedPlayerQuery()
        { }
    }
}