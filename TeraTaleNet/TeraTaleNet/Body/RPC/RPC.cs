namespace TeraTaleNet
{
    public enum RPCType
    {
        Self = 1,
        Others = 2,
        Buffered = 4,
        All = Self | Others,
        AllBuffered = Self | Others | Buffered,
        Specific = 8,
    }

    public abstract class RPC : Body
    {
        public RPCType rpcType;
        public int signallerID;
        public string sender;

        public RPC(RPCType rpcType)
        {
            this.rpcType = rpcType;
            GetType();
        }

        public RPC(byte[] data)
            : base(data)
        { }
    }
}