namespace TeraTaleNet
{
    public class LoginRequest : Body
    {
        public string id;
        public string pw;
        public string confirmID;

        public LoginRequest(string id, string pw, string confirmID)
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