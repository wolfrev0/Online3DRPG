using System;
using System.Text;

namespace TeraTaleNet
{
    public class WriteConsoleRequest : IBody
    {
        string _text;

        public string text { get { return _text; } }

        public WriteConsoleRequest(string text)
        {
            _text = text;
        }

        public WriteConsoleRequest(byte[] buffer)
        {
            Deserialize(buffer);
        }
        
        public Header CreateHeader()
        {
            return new Header(PacketType.WriteConsoleRequest, sizeof(int) + Encoding.UTF8.GetByteCount(text));
        }

        public byte[] Serialize()
        {
            var lenBytes = BitConverter.GetBytes(text.Length);
            var textBytes = Encoding.UTF8.GetBytes(text);

            var ret = new byte[lenBytes.Length + textBytes.Length];

            int offset = 0;
            lenBytes.CopyTo(ret, offset);
            offset += lenBytes.Length;
            textBytes.CopyTo(ret, offset);

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            int len = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _text = Encoding.UTF8.GetString(buffer, offset, len);
        }
    }
}
