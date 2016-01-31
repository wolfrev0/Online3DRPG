using System;

namespace TeraTaleNet
{
    [Flags]
    public enum RPCType : byte
    {
        Self = 0x1,
        Others = 0x2,
        All = Self | Others,
        Buffered = 0x4,
        AllBuffered = All | Buffered,
        Specific = 0x8,
    }

    public abstract class RPC : Body
    {
        public RPCType rpcType;
        public int signallerID;
        public string sender;
        public string receiver = "";

        public RPC(RPCType rpcType)
        {
            this.rpcType = rpcType;
            GetType();
        }

        public RPC(RPCType rpcType, string receiver)
        {
            this.rpcType = rpcType;
            this.receiver = receiver;
            GetType();
        }

        public RPC()
        { }
    }
}