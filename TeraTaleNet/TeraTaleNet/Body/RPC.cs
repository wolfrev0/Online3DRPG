namespace TeraTaleNet
{
    public enum RPCType
    {
        All,
        AllBuffered,
        Others,
        OthersBuffered,
        Server,
    }

    public class RPC : Body
    {
        public string name;

        public RPC(string name)
        {
            this.name = name;
        }

        public RPC(byte[] data)
            : base(data)
        { }
    }
}