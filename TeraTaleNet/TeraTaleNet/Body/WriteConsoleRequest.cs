using LoboNet;

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
            return new Header(PacketType.WriteConsoleRequest, text.SerializedSizeUTF8());
        }

        public byte[] Serialize()
        {
            var textBytes = text.SerializeUTF8();

            var ret = new byte[textBytes.Length];

            int offset = 0;
            textBytes.CopyTo(ret, offset);
            offset += textBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _text = text.DeserializeUTF8(buffer, offset);
            offset += text.SerializedSizeUTF8();
        }
    }
}
