namespace TeraTaleNet
{
    public class SignUp : Body
    {
        public string id;
        public string pw;

        public SignUp(string id, string pw)
        {
            this.id = id;
            this.pw = pw;
        }

        public SignUp()
        { }
    }
}