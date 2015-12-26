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
                if (field.FieldType.IsEnum)
                    buffer = Serializer.Serialize((int)field.GetValue(this));
                else
                {
                    if(!serializersCache.ContainsKey(field.FieldType))
                        serializersCache.Add(field.FieldType, typeof(Serializer).GetMethod("Serialize", new[] { field.FieldType }));
                    buffer = (byte[])serializersCache[field.FieldType].Invoke(null, new[] { field.GetValue(this) });
                }
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
                if (field.FieldType.IsEnum)
                    value = Serializer.ToInt32(buffer, offset);
                else
                    value = typeof(Serializer).GetMethod("To" + field.FieldType.Name, new [] { typeof(byte[]), typeof(int) }).Invoke(null, new object[] { buffer, offset });
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
            if (field.FieldType.IsEnum)
                ret += sizeof(int);
            else
            {
                if (!serializedSizesCache.ContainsKey(field.FieldType))
                    serializedSizesCache.Add(field.FieldType, typeof(Serializer).GetMethod("SerializedSize", new[] { field.FieldType }));
                ret += (int)serializedSizesCache[field.FieldType].Invoke(null, new[] { field.GetValue(this) });
            }
            return ret;
        }
    }
}