using System;
using System.Text;

namespace TeraTaleNet
{
    public class LoginRequest : IBody
    {
        string _id;
        string _pw;

        public string id { get { return _id; } }
        public string pw { get { return _pw; } }

        public LoginRequest(string id, string pw)
        {
            _id = id;
            _pw = pw;
        }

        public LoginRequest(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.LoginRequest, sizeof(int) + Encoding.UTF8.GetByteCount(_id) + sizeof(int) + Encoding.UTF8.GetByteCount(_pw));
        }

        public byte[] Serialize()
        {
            var idLenBytes = BitConverter.GetBytes(id.Length);
            var idBytes = Encoding.UTF8.GetBytes(id);
            var pwLenBytes = BitConverter.GetBytes(pw.Length);
            var pwBytes = Encoding.UTF8.GetBytes(pw);

            var ret = new byte[idLenBytes.Length + idBytes.Length+ pwLenBytes.Length + pwBytes.Length];

            int offset = 0;
            idLenBytes.CopyTo(ret, offset);
            offset += idLenBytes.Length;
            idBytes.CopyTo(ret, offset);
            offset += idBytes.Length;
            pwLenBytes.CopyTo(ret, offset);
            offset += pwLenBytes.Length;
            pwBytes.CopyTo(ret, offset);
            offset += pwBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            int len = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _id = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;
            len = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _pw = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;
        }
    }
}
