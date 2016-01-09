﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace TeraTaleNet
{
    public abstract class Serializable : ISerializable
    {
        static Dictionary<Type, MethodInfo> serializersCache = new Dictionary<Type, MethodInfo>();
        static Dictionary<Type, MethodInfo> serializedSizesCache = new Dictionary<Type, MethodInfo>();

        protected Serializable() { }
        protected Serializable(byte[] data) { Deserialize(data); }

        public byte[] Serialize()
        {
            List<byte[]> buffers = new List<byte[]>();
            int totalBufferSize = 0;
            foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                byte[] buffer;
                var fieldType = field.FieldType;
                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);
                if (!serializersCache.ContainsKey(fieldType))
                    serializersCache.Add(fieldType, typeof(Serializer).GetMethod("Serialize", new[] { fieldType }));
                buffer = (byte[])serializersCache[fieldType].Invoke(null, new[] { Convert.ChangeType(field.GetValue(this), fieldType) });
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

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                object value;
                var fieldType = field.FieldType;
                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);
                value = typeof(Serializer).GetMethod("To" + fieldType.Name, new [] { typeof(byte[]), typeof(int) }).Invoke(null, new object[] { buffer, offset });
                field.SetValue(this, value);
                offset += SerializedSize(field);
            }
        }

        public int SerializedSize()
        {
            int ret = 0;
            foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                ret += SerializedSize(field);
            return ret;
        }

        int SerializedSize(FieldInfo field)
        {
            int ret = 0;
            var fieldType = field.FieldType;
            if (fieldType.IsEnum)
                fieldType = Enum.GetUnderlyingType(fieldType);
            if (!serializedSizesCache.ContainsKey(fieldType))
                serializedSizesCache.Add(fieldType, typeof(Serializer).GetMethod("SerializedSize", new[] { fieldType }));
            ret += (int)serializedSizesCache[fieldType].Invoke(null, new[] { Convert.ChangeType(field.GetValue(this), fieldType) });
            return ret;
        }
    }
    /*bool byte byte[] char double float int long short uint ulong ushort string Vector3 Packet*/
}