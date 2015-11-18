namespace TeraTaleNet
{
    public class LoginRequest : IBody
    {
        string _id;
        string _pw;
        int _confirmID = -1;

        public string id { get { return _id; } }
        public string pw { get { return _pw; } }
        public int confirmID { get { return _confirmID; } set { _confirmID = value; } }

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
            return new Header(PacketType.LoginRequest, SerializedSize());
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += Serializer.SerializedSize(id);
            ret += Serializer.SerializedSize(pw);
            ret += Serializer.SerializedSize(confirmID);
            return ret;
        }

        public byte[] Serialize()
        {
            var idBytes = Serializer.Serialize(id);
            var pwBytes = Serializer.Serialize(pw);
            var confirmIdBytes = Serializer.Serialize(confirmID);

            var ret = new byte[SerializedSize()];

            int offset = 0;
            idBytes.CopyTo(ret, offset);
            offset += idBytes.Length;
            pwBytes.CopyTo(ret, offset);
            offset += pwBytes.Length;
            confirmIdBytes.CopyTo(ret, offset);
            offset += confirmIdBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _id = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(id);
            _pw = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(pw);
            _confirmID = Serializer.ToInt(buffer, offset);
            offset += Serializer.SerializedSize(confirmID);
        }
    }
}