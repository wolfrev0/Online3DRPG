namespace TeraTaleNet
{
    public enum RejectedReason
    {
        Accepted,
        InvalidID,
        InvalidPW,
        LoggedInAlready,
    }

    public class LoginResponse : Body
    {
        public bool accepted;
        public RejectedReason reason;
        public string nickName;
        public int confirmID;

        public LoginResponse(bool accepted, RejectedReason reason, string nickName, int confirmID)
        {
            this.accepted = accepted;
            this.reason = reason;
            this.nickName = nickName;
            this.confirmID = confirmID;
        }

        public LoginResponse(byte[] data)
            : base(data)
        { }
    }
}