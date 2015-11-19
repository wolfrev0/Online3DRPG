namespace TeraTaleNet
{
    public class WriteConsoleRequest : Body
    {
        public string text;

        public WriteConsoleRequest(string text)
        {
            this.text = text;
        }

        public WriteConsoleRequest(byte[] bytes)
            : base(bytes)
        { }

        protected override PacketType Type()
        {
            return PacketType.WriteConsoleRequest;
        }
    }
}