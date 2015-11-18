using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public static class Serializer
    {
        public static byte[] Serialize(bool obj)
        {
            return BitConverter.GetBytes(obj);
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

        public static int SerializedSize(bool obj)
        {
            return sizeof(bool);
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
            return SerializedSize(obj.Length) + Encoding.UTF8.GetByteCount(obj);
        }

        public static bool ToBool(byte[] buffer, int offset)
        {
            return BitConverter.ToBoolean(buffer, 0);
        }

        public static char ToChar(byte[] buffer, int offset)
        {
            return BitConverter.ToChar(buffer, offset);
        }

        public static double ToDouble(byte[] buffer, int offset)
        {
            return BitConverter.ToDouble(buffer, offset);
        }

        public static float ToFloat(byte[] buffer, int offset)
        {
            return BitConverter.ToSingle(buffer, offset);
        }

        public static int ToInt(byte[] buffer, int offset)
        {
            return BitConverter.ToInt32(buffer, offset);
        }

        public static long ToLong(byte[] buffer, int offset)
        {
            return BitConverter.ToInt64(buffer, offset);
        }

        public static short ToShort(byte[] buffer, int offset)
        {
            return BitConverter.ToInt16(buffer, offset);
        }

        public static uint ToUInt(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt32(buffer, offset);
        }

        public static ulong ToULong(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt64(buffer, offset);
        }

        public static ushort ToUShort(byte[] buffer, int offset)
        {
            return BitConverter.ToUInt16(buffer, offset);
        }

        public static string ToString(byte[] buffer, int offset)
        {
            var len = ToInt(buffer, offset);
            offset += sizeof(int);
            var obj = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;

            return obj;
        }
    }
}