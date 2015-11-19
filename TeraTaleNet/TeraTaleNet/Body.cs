using System;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public abstract class Body : ISerializable
    {
        protected Body()
        { }
        
        protected Body(byte[] bytes)
        {
            Deserialize(bytes);
        }

        protected abstract PacketType Type();
        
        public Header CreateHeader()
        {
            return new Header(Type(), SerializedSize());
        }
        
        public byte[] Serialize()
        {
            List<byte[]> buffers = new List<byte[]>();
            int totalBufferSize = 0;
            foreach (var field in GetType().GetFields())
            {
                if (field.FieldType == typeof(bool))
                {
                    var buffer = Serializer.Serialize((bool)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(char))
                {
                    var buffer = Serializer.Serialize((char)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(double))
                {
                    var buffer = Serializer.Serialize((double)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(float))
                {
                    var buffer = Serializer.Serialize((float)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(int))
                {
                    var buffer = Serializer.Serialize((int)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(long))
                {
                    var buffer = Serializer.Serialize((long)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(short))
                {
                    var buffer = Serializer.Serialize((short)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(uint))
                {
                    var buffer = Serializer.Serialize((uint)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(ulong))
                {
                    var buffer = Serializer.Serialize((ulong)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(ushort))
                {
                    var buffer = Serializer.Serialize((ushort)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType == typeof(string))
                {
                    var buffer = Serializer.Serialize((string)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else if (field.FieldType.IsEnum)
                {
                    var buffer = Serializer.Serialize((int)field.GetValue(this));
                    totalBufferSize += buffer.Length;
                    buffers.Add(buffer);
                }
                else
                {
                    throw new Exception("Invalid body field type. TeraTaleNet.Body only can have field as <bool, char, double, float, int, long, short, uing, ulong, ushort, string, enum> type.");
                }
            }

            byte[] ret = new byte[totalBufferSize];
            int offset = 0;
            foreach(var buffer in buffers)
            {
                buffer.CopyTo(ret, offset);
                offset += buffer.Length;
            }
            
            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            foreach (var field in GetType().GetFields())
            {
                if (field.FieldType == typeof(bool))
                {
                    var value = Serializer.ToBool(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(char))
                {
                    var value = Serializer.ToChar(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(double))
                {
                    var value = Serializer.ToDouble(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(float))
                {
                    var value = Serializer.ToFloat(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(int))
                {
                    var value = Serializer.ToInt(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(long))
                {
                    var value = Serializer.ToLong(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(short))
                {
                    var value = Serializer.ToShort(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(uint))
                {
                    var value = Serializer.ToUInt(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(ulong))
                {
                    var value = Serializer.ToULong(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(ushort))
                {
                    var value = Serializer.ToUShort(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType == typeof(string))
                {
                    var value = Serializer.ToString(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else if (field.FieldType.IsEnum)
                {
                    var value = Serializer.ToInt(buffer, offset);
                    field.SetValue(this, value);
                    offset += Serializer.SerializedSize(value);
                }
                else
                {
                    throw new Exception("Invalid body field type. TeraTaleNet.Body only can have field as <bool, char, double, float, int, long, short, uing, ulong, ushort, string> type.");
                }
            }
        }

        public int SerializedSize()
        {
            int ret = 0;
            foreach (var field in GetType().GetFields())
            {
                if (field.FieldType == typeof(bool))
                {
                    ret += sizeof(bool);
                }
                else if (field.FieldType == typeof(char))
                {
                    ret += sizeof(char);
                }
                else if (field.FieldType == typeof(double))
                {
                    ret += sizeof(double);
                }
                else if (field.FieldType == typeof(float))
                {
                    ret += sizeof(float);
                }
                else if (field.FieldType == typeof(int))
                {
                    ret += sizeof(int);
                }
                else if (field.FieldType == typeof(long))
                {
                    ret += sizeof(long);
                }
                else if (field.FieldType == typeof(short))
                {
                    ret += sizeof(short);
                }
                else if (field.FieldType == typeof(uint))
                {
                    ret += sizeof(uint);
                }
                else if (field.FieldType == typeof(ulong))
                {
                    ret += sizeof(ulong);
                }
                else if (field.FieldType == typeof(ushort))
                {
                    ret += sizeof(ushort);
                }
                else if (field.FieldType == typeof(string))
                {
                    ret += Serializer.SerializedSize((string)field.GetValue(this));
                }
                else if (field.FieldType.IsEnum)
                {
                    ret += sizeof(int);
                }
                else
                {
                    throw new Exception("Invalid body field type. TeraTaleNet.Body only can have field as <bool, char, double, float, int, long, short, uing, ulong, ushort, string> type.");
                }
            }
            return ret;
        }
    }
}