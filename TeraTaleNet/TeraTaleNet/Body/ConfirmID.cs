namespace TeraTaleNet
{
    public class ConfirmID : Body
    {
        public string confirmID;

        public ConfirmID(string confirmID)
        {
            this.confirmID = confirmID;
        }

        public ConfirmID(byte[] data)
            : base(data)
        { }
    }
}