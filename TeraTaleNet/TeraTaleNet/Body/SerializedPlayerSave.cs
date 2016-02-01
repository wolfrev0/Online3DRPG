namespace TeraTaleNet
{
    public class SerializedPlayerSave : Body
    {
        public string player;
        public byte[] bytes;

        public SerializedPlayerSave(string player, byte[] bytes)
        {
            this.player = player;
            this.bytes = bytes;
        }

        public SerializedPlayerSave()
        { }
    }
}