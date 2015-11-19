namespace TeraTaleNet
{
    public class WriteConsoleRequest : Body
    {
        public string text;

        public WriteConsoleRequest(string text)
        {
            this.text = text;
        }

        public WriteConsoleRequest(byte[] data)
            : base(data)
        { }
    }
}