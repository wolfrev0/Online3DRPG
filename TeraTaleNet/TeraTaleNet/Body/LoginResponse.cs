using System;
using System.Text;
using LoboNet;

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

        public bool accepted { get { return _accepted; } set { _accepted = value; } }
        public RejectedReason reason { get { return _reason; } set { _reason = value; } }
        public string nickName { get { return _nickName; } }

        public LoginResponse(bool accepted, RejectedReason reason, string nickName)
        {
            _accepted = accepted;
            _reason = reason;
            _nickName = nickName;
        }

        public LoginResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.LoginResponse, sizeof(bool) + sizeof(int) + nickName.SerializedSizeUTF8());
        }

        public byte[] Serialize()
        {
            var acceptedBytes = accepted.Serialize();
            var reasonBytes = ((int)reason).Serialize();
            var nickNameBytes = nickName.SerializeUTF8();

            var ret = new byte[acceptedBytes.Length + reasonBytes.Length + nickNameBytes.Length];

            int offset = 0;
            acceptedBytes.CopyTo(ret, offset);
            offset += acceptedBytes.Length;
            reasonBytes.CopyTo(ret, offset);
            offset += reasonBytes.Length;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _accepted = BitConverter.ToBoolean(buffer, offset);
            offset += sizeof(bool);
            _reason = (RejectedReason)BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _nickName = nickName.DeserializeUTF8(buffer, offset);
            offset += nickName.SerializedSizeUTF8();
        }
    }
}
