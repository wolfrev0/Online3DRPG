namespace TeraTaleNet
{
    public class PlayerInfoRequest : Body
    {
        public string nickName;

        public PlayerInfoRequest(string nickName)
        {
            this.nickName = nickName;
        }

        public PlayerInfoRequest(byte[] data)
            : base(data)
        { }
    }
}