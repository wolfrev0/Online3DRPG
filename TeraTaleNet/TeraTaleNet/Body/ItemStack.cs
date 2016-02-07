using UnityEngine;
using System;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class ItemStackOverflow : Exception
    { }

    public class ItemTypeMismatch : Exception
    { }

    public class ItemStack : IAutoSerializable
    {
        Stack<Item> _stack = new Stack<Item>(new[] { new ItemNull() });

        public int count { get { return item.isNull ? 0 : _stack.Count; } }
        public Sprite sprite { get { return _stack.Peek().sprite; } }
        public Item item { get { return _stack.Peek(); } }

        public ItemStack()
        { }

        public void Push(Item item)
        {
            Item i = _stack.Peek();
            if (i.isNull || i.IsSameType(item))
            {
                if (IsFull() == false)
                {
                    if (i.isNull)
                        _stack.Pop();
                    _stack.Push(item);
                }
                else
                    throw new ItemStackOverflow();
            }
            else
                throw new ItemTypeMismatch();
        }

        bool IsFull()
        {
            return _stack.Peek().maxCount <= _stack.Count;
        }

        public bool IsPushable(Item item)
        {
            Item i = _stack.Peek();

            if (i.isNull)
                return true;
            return IsFull() == false && i.IsSameType(item);
        }

        public bool IsPushable(Item item, int amount)
        {
            if (IsFull())
                return false;

            Item i = _stack.Peek();

            if (i.isNull)
            {
                if (amount <= item.maxCount)
                    return true;
                return false;
            }
            else
            {
                if (i.IsSameType(item) && _stack.Count + amount <= item.maxCount)
                    return true;
                return false;
            }
        }

        public void Use(object player)
        {
            Item item = _stack.Peek();
            item.Use(player);

            if (item.isConsumables)
                Pop();
        }

        public Item Pop()
        {
            var ret = _stack.Pop();
            if (_stack.Count == 0)
                _stack.Push(new ItemNull());
            return ret;
        }

        public byte[] Serialize()
        {
            var buffer = new byte[SerializedSize()];
            int offset = 0;

            var sizeBytes = Serializer.Serialize(_stack.Count);
            sizeBytes.CopyTo(buffer, offset);
            offset += sizeof(int);

            foreach (var item in _stack)
            {
                var packet = new Packet(item);
                var data = packet.Serialize();
                data.CopyTo(buffer, offset);
                offset += data.Length;
            }
            return buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;

            int count = Serializer.ToInt32(buffer, offset);
            offset += Serializer.SerializedSize(count);

            _stack.Clear();

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
                _stack.Push((Item)packet.body);
            }
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += sizeof(int);
            ret += Header.size * _stack.Count;
            foreach (var item in _stack)
                ret += item.SerializedSize();
            return ret;
        }

        public Header CreateHeader()
        {
            return Serializer.CreateHeader(this);
        }
    }
}