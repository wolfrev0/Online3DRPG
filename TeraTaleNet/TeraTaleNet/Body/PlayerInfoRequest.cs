using LoboNet;

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
            return new Header(PacketType.PlayerInfoRequest, nickName.SerializedSizeUTF8());
        }

        public byte[] Serialize()
        {
            var nickNameBytes = nickName.SerializeUTF8();

            var ret = new byte[nickNameBytes.Length];

            int offset = 0;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _nickName = nickName.DeserializeUTF8(buffer, offset);
            offset += nickName.SerializedSizeUTF8();
        }
    }
}