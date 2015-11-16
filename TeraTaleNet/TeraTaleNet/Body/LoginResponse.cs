using System;
using System.Text;
using LoboNet;

namespace TeraTaleNet
{
    public class LoginResponse : IBody
    {
        bool _accepted;
        string _nickName;

        public bool accepted { get { return _accepted; } }
        public string nickName { get { return _nickName; } }

        public LoginResponse(bool accepted, string nickName)
        {
            _accepted = accepted;
            _nickName = nickName;
        }

        public LoginResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.LoginResponse, sizeof(bool) + nickName.SerializedSizeUTF8());
        }

        public byte[] Serialize()
        {
            var acceptedBytes = accepted.Serialize();
            var nickNameBytes = nickName.SerializeUTF8();

            var ret = new byte[acceptedBytes.Length + nickNameBytes.Length];

            int offset = 0;
            acceptedBytes.CopyTo(ret, offset);
            offset += acceptedBytes.Length;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _accepted = BitConverter.ToBoolean(buffer, offset);
            offset += sizeof(bool);
            _nickName = nickName.DeserializeUTF8(buffer, offset);
            offset += nickName.SerializedSizeUTF8();
        }
    }
}
