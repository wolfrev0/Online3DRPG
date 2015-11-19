namespace TeraTaleNet
{
    public class LoginRequest : Body
    {
        public string id;
        public string pw;
        public int confirmID = -1;

        public LoginRequest(string id, string pw)
        {
            this.id = id;
            this.pw = pw;
        }

        public LoginRequest(byte[] data)
            : base(data)
        { }
    }
}