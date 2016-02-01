
using System;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class ItemStackList : IAutoSerializable
    {
        List<ItemStack> _list;

        public ItemStackList()
            : this(0)
        { }

        public ItemStackList(int capacity)
        { _list = new List<ItemStack>(capacity); }

        public ItemStack this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public void Add(ItemStack item)
        { _list.Add(item); }

        public ItemStack Find(Predicate<ItemStack> p)
        { return _list.Find(p); }

        public Header CreateHeader()
        {
            return Serializer.CreateHeader(this);
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;

            int count = Serializer.ToInt32(buffer, offset);
            offset += Serializer.SerializedSize(count);

            _list.Clear();

            var bytes = new byte[1024];
            for (int i = 0; i < count; i++)
            {
                Array.Copy(buffer, offset, bytes, 0, Header.size);
                var header = new Header();
                header.Deserialize(bytes);
                offset += Header.size;
                Array.Copy(buffer, offset, bytes, 0, header.bodySize);
                var packet = Packet.Create(header, bytes);
                offset += header.bodySize;
                _list.Add((ItemStack)packet.body);
            }
        }

        public byte[] Serialize()
        {
            var buffer = new byte[SerializedSize()];
            int offset = 0;

            var sizeBytes = Serializer.Serialize(_list.Count);
            sizeBytes.CopyTo(buffer, offset);
            offset += sizeof(int);

            foreach (var item in _list)
            {
                var packet = new Packet(item);
                var data = packet.Serialize();
                data.CopyTo(buffer, offset);
                offset += data.Length;
            }
            return buffer;
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += sizeof(int);
            ret += Header.size * _list.Count;
            foreach (var item in _list)
                ret += item.SerializedSize();
            return ret;
        }
    }
}