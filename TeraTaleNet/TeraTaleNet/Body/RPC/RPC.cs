namespace TeraTaleNet
{
    public enum RPCType
    {
        Self = 0x1,
        Others = 0x2,
        Server = 0x4,
        Buffered = 0x8,
        All = Self | Others | Server,
        AllBuffered = Self | Others | Server | Buffered,
    }

    public abstract class RPC : Body
    {
        public RPCType rpcType;
        public int signallerID;
        public string sender;

        public RPC(RPCType rpcType)
        {
            this.rpcType = rpcType;
        }

        public RPC(byte[] data)
            : base(data)
        { }
    }
}