namespace TeraTaleNet
{
    public class RemoteProcedureCall : Body
    {
        public string name;

        public RemoteProcedureCall(string name)
        {
            this.name = name;
        }

        public RemoteProcedureCall(byte[] data)
            : base(data)
        { }
    }
}