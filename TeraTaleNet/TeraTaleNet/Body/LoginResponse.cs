using System;
using System.Text;

namespace TeraTaleNet
{
    public class LoginResponse : IBody
    {
        bool _accepted;

        public bool accepted { get { return _accepted; } }

        public LoginResponse(bool accepted)
        {
            _accepted = accepted;
        }

        public LoginResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.LoginResponse, sizeof(bool));
        }

        public byte[] Serialize()
        {
            var bytes = BitConverter.GetBytes(accepted);

            var ret = new byte[bytes.Length];

            int offset = 0;
            bytes.CopyTo(ret, offset);
            offset += bytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _accepted = BitConverter.ToBoolean(buffer, offset);
            offset += sizeof(bool);
        }
    }
}
