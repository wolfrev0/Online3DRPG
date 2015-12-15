namespace TeraTaleNet
{
    public enum RPCTarget
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
        public RPCTarget target;
        public int signallerID;

        public RPC(RPCTarget target, int signallerID)
        {
            this.target = target;
            this.signallerID = signallerID;
        }

        public RPC(byte[] data)
            : base(data)
        { }
    }
}