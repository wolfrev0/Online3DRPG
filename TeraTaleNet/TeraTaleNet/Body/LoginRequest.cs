namespace TeraTaleNet
{
    public class LoginRequest : Body
    {
        public string id;
        public string pw;
        public int confirmID;

        public LoginRequest(string id, string pw, int confirmID)
        {
            this.id = id;
            this.pw = pw;
            this.confirmID = confirmID;
        }

        public LoginRequest(byte[] data)
            : base(data)
        { }
    }
}