namespace TeraTaleNet
{
    public class ConfirmID : Body
    {
        public int id;

        public ConfirmID(int id)
        {
            this.id = id;
        }

        public ConfirmID(byte[] data)
            : base(data)
        { }
    }
}