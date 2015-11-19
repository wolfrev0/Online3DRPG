namespace TeraTaleNet
{
    public class PlayerLogin : Body
    {
        public string nickName;

        public PlayerLogin(string nickName)
        {
            this.nickName = nickName;
        }

        public PlayerLogin(byte[] bytes)
            : base(bytes)
        { }

        protected override PacketType Type()
        {
            return PacketType.PlayerLogin;
        }
    }
}