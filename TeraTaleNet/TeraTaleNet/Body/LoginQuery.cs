namespace TeraTaleNet
{
    public class LoginQuery : Body
    {
        public string id;
        public string pw;
        public int confirmID;

        public LoginQuery(string id, string pw, int confirmID)
        {
            this.id = id;
            this.pw = pw;
            this.confirmID = confirmID;
        }

        public LoginQuery()
        { }
    }
}