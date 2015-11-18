namespace TeraTaleNet
{
    public enum RejectedReason
    {
        Accepted,
        InvalidID,
        InvalidPW,
        LoggedInAlready,
    }

    public class LoginResponse : IBody
    {
        bool _accepted;
        RejectedReason _reason;
        string _nickName;
        int _confirmID;

        public bool accepted { get { return _accepted; } set { _accepted = value; } }
        public RejectedReason reason { get { return _reason; } set { _reason = value; } }
        public string nickName { get { return _nickName; } }
        public int confirmID { get { return _confirmID; } set { _confirmID = value; } }

        public LoginResponse(bool accepted, RejectedReason reason, string nickName, int confirmID)
        {
            _accepted = accepted;
            _reason = reason;
            _nickName = nickName;
            _confirmID = confirmID;
        }

        public LoginResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.LoginResponse, SerializedSize());
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += Serializer.SerializedSize(accepted);
            ret += Serializer.SerializedSize((int)reason);
            ret += Serializer.SerializedSize(nickName);
            ret += Serializer.SerializedSize(confirmID);
            return ret;
        }

        public byte[] Serialize()
        {
            var acceptedBytes = Serializer.Serialize(accepted);
            var reasonBytes = Serializer.Serialize((int)reason);
            var nickNameBytes = Serializer.Serialize(nickName);
            var confirmIdBytes = Serializer.Serialize(confirmID);

            var ret = new byte[SerializedSize()];

            int offset = 0;
            acceptedBytes.CopyTo(ret, offset);
            offset += acceptedBytes.Length;
            reasonBytes.CopyTo(ret, offset);
            offset += reasonBytes.Length;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;
            confirmIdBytes.CopyTo(ret, offset);
            offset += confirmIdBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _accepted = Serializer.ToBool(buffer, offset);
            offset += Serializer.SerializedSize(accepted);
            _reason = (RejectedReason)Serializer.ToInt(buffer, offset);
            offset += Serializer.SerializedSize((int)reason);
            _nickName = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(nickName);
            _confirmID = Serializer.ToInt(buffer, offset);
            offset += Serializer.SerializedSize(confirmID);
        }
    }
}
