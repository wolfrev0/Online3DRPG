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
            return new Header(PacketType.LoginResponse, sizeof(bool) + sizeof(int) + nickName.SerializedSizeUTF8() + sizeof(int));
        }

        public byte[] Serialize()
        {
            var acceptedBytes = accepted.Serialize();
            var reasonBytes = ((int)reason).Serialize();
            var nickNameBytes = nickName.SerializeUTF8();
            var confirmIdBytes = confirmID.Serialize();

            var ret = new byte[acceptedBytes.Length + reasonBytes.Length + nickNameBytes.Length + confirmIdBytes.Length];

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
            _accepted = BitConverter.ToBoolean(buffer, offset);
            offset += sizeof(bool);
            _reason = (RejectedReason)BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            _nickName = nickName.DeserializeUTF8(buffer, offset);
            offset += nickName.SerializedSizeUTF8();
            _confirmID = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
        }
    }
}
