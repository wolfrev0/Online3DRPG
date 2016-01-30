using System;
using System.Collections.Generic;
using System.Linq;

namespace TeraTaleNet
{
    public abstract class Body : Serializable
    {
        protected Body()
        { }

        protected Body(byte[] data)
            : base(data)
        { }

        public static implicit operator Packet(Body body)
        {
            return new Packet(body);
        }
    }
}