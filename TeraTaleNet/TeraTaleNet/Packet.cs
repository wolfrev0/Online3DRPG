using System;

namespace TeraTaleNet
{
    public class Packet
    {
        static string bodyNamespace = typeof(Body).Namespace;

        Header _header;
        Body _body;

        public Header header { get { return _header; } }
        public Body body { get { return _body; } }

        static public Packet Create(Header header, byte[] bytes)
        {
            Body body = (Body)Activator.CreateInstance(Type.GetType(bodyNamespace + "." + Body.GetNameByIndex(header.type)), bytes);
            return new Packet(header, body);
        }

        Packet(Header header, Body body)
        {
            _header = header;
            _body = body;
        }

        public Packet(Body body)
        {
            _header = body.CreateHeader();
            _body = body;
        }
    }
}
