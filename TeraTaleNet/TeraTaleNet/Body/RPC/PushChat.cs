namespace TeraTaleNet
{
    public class PushChat : RPC
    {
        public string chat;

        public PushChat(RPCType rpcType, string chat)
            : base(rpcType)
        {
            this.chat = chat;
        }

        public PushChat(byte[] data)
            : base(data)
        { }
    }
}