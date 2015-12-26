namespace TeraTaleNet
{
    public class LoginAnswer : Body
    {
        public int confirmID;
        public bool accepted;
        public string name;
        public string world;

        public LoginAnswer(int confirmID, bool accepted, string name, string world)
        {
            this.confirmID = confirmID;
            this.accepted = accepted;
            this.name = name;
            this.world = world;
        }

        public LoginAnswer(byte[] data)
            : base(data)
        { }
    }
}