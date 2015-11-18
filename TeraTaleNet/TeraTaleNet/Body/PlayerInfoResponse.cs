using LoboNet;

namespace TeraTaleNet
{
    public class PlayerInfoResponse : IBody
    {
        string _nickName;

        public string nickName { get { return _nickName; } }

        public PlayerInfoResponse(string nickName)
        {
            _nickName = nickName;
        }

        public PlayerInfoResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.PlayerInfoResponse, nickName.SerializedSizeUTF8());
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