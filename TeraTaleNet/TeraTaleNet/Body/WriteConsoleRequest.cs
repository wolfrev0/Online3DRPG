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
            return new Header(PacketType.WriteConsoleRequest, SerializedSize());
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += Serializer.SerializedSize(text);
            return ret;
        }

        public byte[] Serialize()
        {
            var textBytes = Serializer.Serialize(text);

            var ret = new byte[SerializedSize()];

            int offset = 0;
            textBytes.CopyTo(ret, offset);
            offset += textBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _text = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(text);
        }
    }
}
