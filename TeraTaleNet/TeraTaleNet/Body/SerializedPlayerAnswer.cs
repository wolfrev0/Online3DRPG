namespace TeraTaleNet
{
    public class SerializedPlayerAnswer : Body
    {
        public string player;
        public byte[] bytes;

        public SerializedPlayerAnswer(string player, byte[] bytes)
        {
            this.player = player;
            this.bytes = bytes;
        }

        public SerializedPlayerAnswer()
        { }
    }
}