namespace TeraTaleNet
{
    public class PlayerLogin : Body
    {
        public string nickName;

        public PlayerLogin(string nickName)
        {
            this.nickName = nickName;
        }

        public PlayerLogin(byte[] data)
            : base(data)
        { }
    }
}