﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace TeraTaleNet
{
    public abstract class Serializable : IAutoSerializable
    {
        protected Serializable() { }

        public byte[] Serialize()
        {
            return Serializer.Serialize(this);
        }

        public void Deserialize(byte[] buffer)
        {
            Serializer.Deserialize(this, buffer);
        }

        public int SerializedSize()
        {
            return Serializer.SerializedSize(this);
        }

        public Header CreateHeader()
        {
            return Serializer.CreateHeader(this);
        }
    }
}