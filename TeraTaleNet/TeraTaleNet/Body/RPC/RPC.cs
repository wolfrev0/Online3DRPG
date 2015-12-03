namespace TeraTaleNet
{
    public enum RPCType
    {
        Self = 0x1,
        Others = 0x2,
        Server = 0x4,
        All = Self | Others | Server,
        //Buffered

        //All,
        //AllBuffered,
        //Others,
        //OthersBuffered,
        //Server,
    }

    public abstract class RPC : Body
    {
        public RPCType type;

        public RPC(RPCType type)
        {
            this.type = type;
        }

        public RPC(byte[] data)
            : base(data)
        { }
    }
}