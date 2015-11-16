using System;
using System.Text;
using LoboNet;

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
            return new Header(PacketType.LoginRequest, id.SerializedSizeUTF8() + pw.SerializedSizeUTF8());
        }

        public byte[] Serialize()
        {
            var idBytes = id.SerializeUTF8();
            var pwBytes = pw.SerializeUTF8();

            var ret = new byte[idBytes.Length + pwBytes.Length];

            int offset = 0;
            idBytes.CopyTo(ret, offset);
            offset += idBytes.Length;
            pwBytes.CopyTo(ret, offset);
            offset += pwBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _id += id.DeserializeUTF8(buffer, offset);
            offset += id.SerializedSizeUTF8();
            _pw += pw.DeserializeUTF8(buffer, offset);
            offset += pw.SerializedSizeUTF8();
        }
    }
}
