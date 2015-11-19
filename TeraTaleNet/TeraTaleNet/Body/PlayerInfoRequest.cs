namespace TeraTaleNet
{
    public class PlayerInfoRequest : Body
    {
        public string nickName;

        public PlayerInfoRequest(string nickName)
        {
            this.nickName = nickName;
        }

        public PlayerInfoRequest(byte[] bytes)
            : base(bytes)
        { }

        protected override PacketType Type()
        {
            return PacketType.PlayerInfoRequest;
        }
    }
}