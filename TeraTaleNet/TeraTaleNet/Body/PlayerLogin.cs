using LoboNet;

namespace TeraTaleNet
{
    public class PlayerLogin : IBody
    {
        string _nickName;

        public string nickName { get { return _nickName; } }

        public PlayerLogin(string nickName)
        {
            _nickName = nickName;
        }

        public PlayerLogin(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.PlayerLogin, SerializedSize());
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += Serializer.SerializedSize(nickName);
            return ret;
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