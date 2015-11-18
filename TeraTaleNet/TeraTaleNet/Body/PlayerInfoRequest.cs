namespace TeraTaleNet
{
    public class PlayerInfoRequest : IBody
    {
        string _nickName;

        public string nickName { get { return _nickName; } }

        public PlayerInfoRequest(string nickName)
        {
            _nickName = nickName;
        }

        public PlayerInfoRequest(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.PlayerInfoRequest, SerializedSize());
        }

        public int SerializedSize()
        {
            return Serializer.SerializedSize(_nickName);
        }

        public byte[] Serialize()
        {
            var nickNameBytes = Serializer.Serialize(nickName);

            var ret = new byte[SerializedSize()];

            int offset = 0;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _nickName = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(nickName);
        }
    }
}