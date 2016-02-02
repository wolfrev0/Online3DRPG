namespace TeraTaleNet
{
    public class BufferedRPCRequest : Body
    {
        public string caller;

        public BufferedRPCRequest(string caller)
        {
            this.caller = caller;
        }

        public BufferedRPCRequest()
        { }
    }
}