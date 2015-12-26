using System;
using System.Collections.Generic;
using System.Linq;

namespace TeraTaleNet
{
    public abstract class Body : Serializable
    {
        static Dictionary<string, int> _indexByType = new Dictionary<string, int>();
        static Dictionary<int, string> _nameByIndex = new Dictionary<int, string>();

        static Body()
        {
            var type = typeof(Body);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
            var typeArr = types.ToArray();
            for (int i = 0; i < typeArr.Length; i++)
            {
                _indexByType.Add(typeArr[i].Name, i);
                _nameByIndex.Add(i, typeArr[i].Name);
            }
        }

        static public int GetIndexByName(string type)
        {
            return _indexByType[type];
        }

        static public string GetNameByIndex(int index)
        {
            return _nameByIndex[index];
        }

        protected Body()
        { }

        protected Body(byte[] data)
            : base(data)
        { }

        public Header CreateHeader()
        {
            return new Header(GetIndexByName(GetType().Name), SerializedSize());
        }

        public static implicit operator Packet(Body body)
        {
            return new Packet(body);
        }
    }
}