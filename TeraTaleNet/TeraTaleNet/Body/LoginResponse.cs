using System;
using System.Text;

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
            return new Header(PacketType.LoginResponse, sizeof(bool) + sizeof(int) + Encoding.UTF8.GetByteCount(nickName));
        }

        public byte[] Serialize()
        {
            var acceptedBytes = BitConverter.GetBytes(accepted);
            var nickNameLenBytes = BitConverter.GetBytes(nickName.Length);
            var nickNameBytes = Encoding.UTF8.GetBytes(nickName);

            var ret = new byte[acceptedBytes.Length + nickNameLenBytes.Length + nickNameBytes.Length];

            int offset = 0;
            acceptedBytes.CopyTo(ret, offset);
            offset += acceptedBytes.Length;
            nickNameLenBytes.CopyTo(ret, offset);
            offset += nickNameLenBytes.Length;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _accepted = BitConverter.ToBoolean(buffer, offset);
            offset += sizeof(bool);
            int len = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _nickName = Encoding.UTF8.GetString(buffer, offset, len);
            offset += Encoding.UTF8.GetByteCount(nickName);
        }
    }
}
