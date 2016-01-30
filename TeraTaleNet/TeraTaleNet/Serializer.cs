using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TeraTaleNet
{
    public static class Serializer
    {
        static Dictionary<Type, MethodInfo> serializersCache = new Dictionary<Type, MethodInfo>();
        static Dictionary<Type, MethodInfo> serializedSizesCache = new Dictionary<Type, MethodInfo>();

        public static byte[] Serialize(bool obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(byte obj)
        {
            return new[] { obj };
        }

        public static byte[] Serialize(byte[] obj)
        {
            var bytes = obj;
            var lenBytes = Serialize(bytes.Length);

            var ret = new byte[lenBytes.Length + bytes.Length];

            int offset = 0;
            lenBytes.CopyTo(ret, offset);
            offset += lenBytes.Length;
            bytes.CopyTo(ret, offset);
            offset += bytes.Length;

            return ret;
        }

        public static byte[] Serialize(char obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(double obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(float obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(int obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(long obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(short obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(uint obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(ulong obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(ushort obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(string obj)
        {
            var bytes = Encoding.UTF8.GetBytes(obj);
            var lenBytes = Serialize(bytes.Length);

            var ret = new byte[lenBytes.Length + bytes.Length];

            int offset = 0;
            lenBytes.CopyTo(ret, offset);
            offset += lenBytes.Length;
            bytes.CopyTo(ret, offset);
            offset += bytes.Length;

            return ret;
        }

        public static byte[] Serialize(Vector3 obj)
        {
            var ret = new byte[12];
            BitConverter.GetBytes(obj.x).CopyTo(ret, 0);
            BitConverter.GetBytes(obj.y).CopyTo(ret, 4);
            BitConverter.GetBytes(obj.z).CopyTo(ret, 8);
            return ret;
        }

        public static byte[] Serialize(Packet obj)
        {
            return obj.Serialize();
        }

        public static int SerializedSize(bool obj)
        {
            return sizeof(bool);
        }

        public static int SerializedSize(byte obj)
        {
            return sizeof(byte);
        }

        public static int SerializedSize(byte[] obj)
        {
            return sizeof(int) + obj.Length;
        }

        public static int SerializedSize(char obj)
        {
            return sizeof(char);
        }

        public static int SerializedSize(double obj)
        {
            return sizeof(double);
        }

        public static int SerializedSize(float obj)
        {
            return sizeof(float);
        }

        public static int SerializedSize(int obj)
        {
            return sizeof(int);
        }

        public static int SerializedSize(long obj)
        {
            return sizeof(long);
        }

        public static int SerializedSize(short obj)
        {
            return sizeof(short);
        }

        public static int SerializedSize(uint obj)
        {
            return sizeof(uint);
        }

        public static int SerializedSize(ulong obj)
        {
            return sizeof(ulong);
        }

        public static int SerializedSize(ushort obj)
        {
            return sizeof(ushort);
        }

        public static int SerializedSize(string obj)
        {
            return sizeof(int) + Encoding.UTF8.GetByteCount(obj);
        }

        public static int SerializedSize(Vector3 obj)
        {
            return sizeof(float) * 3;
        }

        public static int SerializedSize(Packet obj)
        {
            return obj.SerializedSize();
        }

        public static bool ToBoolean(byte[] buffer, int offset)
        {
            return BitConverter.ToBoolean(buffer, offset);
        }

        public static byte ToByte(byte[] buffer, int offset)
        {
            return buffer[offset];
        }

        public static byte[] ToBytes(byte[] buffer, int offset)
        {
            var len = ToInt32(buffer, offset);
            offset += sizeof(int);

            byte[] ret = new byte[len];
            Buffer.BlockCopy(buffer, offset, ret, 0, len);
            offset += len;

            return ret;
        }

        public static char ToChar(byte[] buffer, int offset)
        {
            return BitConverter.ToChar(buffer, offset);
        }

        public static double ToDouble(byte[] buffer, int offset)
        {
            return BitConverter.ToDouble(buffer, offset);
        }

        public static float ToSingle(byte[] buffer, int offset)
        {
            return BitConverter.ToSingle(buffer, offset);
        }

        public static int ToInt32(byte[] buffer, int offset)
        {
            return BitConverter.ToInt32(buffer, offset);
        }

        public static long ToInt64(byte[] buffer, int offset)
        {
            return BitConverter.ToInt64(buffer, offset);
        }

        public static short ToInt16(byte[] buffer, int offset)
        {
            return BitConverter.ToInt16(buffer, offset);
        }

        public static uint ToUInt32(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt32(buffer, offset);
        }

        public static ulong ToUInt64(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt64(buffer, offset);
        }

        public static ushort ToUInt16(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt16(buffer, offset);
        }

        public static string ToString(byte[] buffer, int offset)
        {
            var len = ToInt32(buffer, offset);
            offset += sizeof(int);
            var obj = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;

            return obj;
        }

        public static Vector3 ToVector3(byte[] buffer, int offset)
        {
            var x = ToSingle(buffer, offset);
            offset += SerializedSize(x);
            var y = ToSingle(buffer, offset);
            offset += SerializedSize(y);
            var z = ToSingle(buffer, offset);
            offset += SerializedSize(z);

            return new Vector3(x, y, z);
        }

        public static Packet ToPacket(byte[] buffer, int offset)
        {
            byte[] bytes = new byte[Header.size];
            Buffer.BlockCopy(buffer, offset, bytes, 0, Header.size);
            Header header = new Header(bytes);
            offset += Header.size;
            bytes = new byte[header.bodySize];
            Buffer.BlockCopy(buffer, offset, bytes, 0, header.bodySize);
            return Packet.Create(header, bytes);
        }

        public static byte[] Serialize(ISerializable obj)
        {
            List<byte[]> buffers = new List<byte[]>();
            int totalBufferSize = 0;
            foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                byte[] buffer;
                var fieldType = field.FieldType;
                var value = field.GetValue(obj);
                if (fieldType.IsEnum)
                {
                    fieldType = Enum.GetUnderlyingType(fieldType);
                    value = Convert.ChangeType(value, fieldType);
                }
                if (fieldType.IsSubclassOf(typeof(ISerializable)) || fieldType == typeof(ISerializable))
                {
                    fieldType = typeof(Packet);
                    value = Activator.CreateInstance(typeof(Packet), field.GetValue(obj));
                }
                if (!serializersCache.ContainsKey(fieldType))
                    serializersCache.Add(fieldType, typeof(Serializer).GetMethod("Serialize", new[] { fieldType }));
                buffer = (byte[])serializersCache[fieldType].Invoke(null, new[] { value });
                totalBufferSize += buffer.Length;
                buffers.Add(buffer);
            }

            byte[] ret = new byte[totalBufferSize];
            int offset = 0;
            foreach (var buffer in buffers)
            {
                buffer.CopyTo(ret, offset);
                offset += buffer.Length;
            }

            return ret;
        } 

        public static void Deserialize(ISerializable obj, byte[] buffer)
        {
            int offset = 0;
            foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                object value;
                var fieldType = field.FieldType;
                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);
                string typeName = fieldType.Name;
                if (fieldType.IsArray)
                    typeName = typeName.Replace("[]", "s");
                if (fieldType.IsSubclassOf(typeof(ISerializable)) || fieldType == typeof(ISerializable))
                    typeName = "Packet";
                value = typeof(Serializer).GetMethod("To" + typeName, new[] { typeof(byte[]), typeof(int) }).Invoke(null, new object[] { buffer, offset });
                if (fieldType.IsSubclassOf(typeof(ISerializable)) || fieldType == typeof(ISerializable))
                    value = value.GetType().GetField("body").GetValue(value);
                field.SetValue(obj, value);
                offset += SerializedSize(obj, field);
            }
        }

        static int SerializedSize(ISerializable obj, FieldInfo field)
        {
            int ret = 0;
            var fieldType = field.FieldType;
            object value = field.GetValue(obj);
            if (fieldType.IsEnum)
            {
                fieldType = Enum.GetUnderlyingType(fieldType);
                value = Convert.ChangeType(value, fieldType);
            }
            if (fieldType.IsSubclassOf(typeof(ISerializable)) || fieldType == typeof(ISerializable))
            {
                fieldType = typeof(Packet);
                value = Activator.CreateInstance(typeof(Packet), field.GetValue(obj));
            }
            if (!serializedSizesCache.ContainsKey(fieldType))
                serializedSizesCache.Add(fieldType, typeof(Serializer).GetMethod("SerializedSize", new[] { fieldType }));
            ret += (int)serializedSizesCache[fieldType].Invoke(null, new[] { value });
            return ret;
        }

        public static int SerializedSize(ISerializable obj)
        {
            int ret = 0;
            foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                ret += SerializedSize(obj, field);
            return ret;
        }

        public static Header CreateHeader(ISerializable obj)
        {
            return new Header(Packet.GetIndexByType(obj.GetType()), SerializedSize(obj));
        }
    }
}