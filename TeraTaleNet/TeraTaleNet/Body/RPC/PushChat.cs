namespace TeraTaleNet
{
    public class PushChat : RPC
    {
        public string chat;

        public PushChat(string chat)
            : base(RPCType.All)
        {
            this.chat = chat;
        }

        public PushChat(byte[] data)
            : base(data)
        { }
    }
}