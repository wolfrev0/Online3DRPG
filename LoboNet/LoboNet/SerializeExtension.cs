using System;
using System.Text;

namespace LoboNet
{
    public static class SerializeExtension
    {
        public static byte[] Serialize(this bool obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this char obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this double obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this float obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this int obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this long obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this short obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this uint obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this ulong obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] Serialize(this ushort obj)
        {
            return BitConverter.GetBytes(obj);
        }

        public static byte[] SerializeUTF8(this string obj)
        {
            var bytes = Encoding.UTF8.GetBytes(obj);
            var lenBytes = bytes.Length.Serialize();

            var ret = new byte[lenBytes.Length + bytes.Length];

            int offset = 0;
            lenBytes.CopyTo(ret, offset);
            offset += lenBytes.Length;
            bytes.CopyTo(ret, offset);
            offset += bytes.Length;

            return ret;
        }

        public static int SerializedSizeUTF8(this string obj)
        {
            return sizeof(int) + Encoding.UTF8.GetByteCount(obj);
        }

        public static string DeserializeUTF8(this string obj, byte[] buffer, int offset)
        {
            int len = BitConverter.ToInt32(buffer, offset);
            offset += sizeof(int);
            obj = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;

            return obj;
        }
    }
}