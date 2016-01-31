using System;

namespace TeraTaleNet
{
    public class Header : Serializable
    {
        static public int size = sizeof(int) + sizeof(int);

        public int type;
        public int bodySize;

        public Header(int type, int bodySize)
        {
            this.type = type;
            this.bodySize = bodySize;
        }

        public Header()
        { }
    }
}