using UnityEngine;

namespace TeraTaleNet
{
    public class SerializableBoolean : Body
    {
        public bool value;

        public SerializableBoolean(bool value)
        {
            this.value = value;
        }

        public SerializableBoolean(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableBoolean(bool value)
        {
            return new SerializableBoolean(value);
        }

        public static implicit operator bool (SerializableBoolean value)
        {
            return value.value;
        }
    }

    public class SerializableByte : Body
    {
        public byte value;

        public SerializableByte(byte value)
        {
            this.value = value;
        }

        public SerializableByte(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableByte(byte value)
        {
            return new SerializableByte(value);
        }

        public static implicit operator byte (SerializableByte value)
        {
            return value.value;
        }
    }

    public class SerializableChar : Body
    {
        public char value;
        
        public SerializableChar(char value)
        {
            this.value = value;
        }

        public SerializableChar(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableChar(char value)
        {
            return new SerializableChar(value);
        }

        public static implicit operator char (SerializableChar value)
        {
            return value.value;
        }
    }

    public class SerializableDouble : Body
    {
        public double value;

        public SerializableDouble(double value)
        {
            this.value = value;
        }

        public SerializableDouble(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableDouble(double value)
        {
            return new SerializableDouble(value);
        }

        public static implicit operator double (SerializableDouble value)
        {
            return value.value;
        }
    }

    public class SerializableSingle : Body
    {
        public float value;

        public SerializableSingle(float value)
        {
            this.value = value;
        }

        public SerializableSingle(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableSingle(float value)
        {
            return new SerializableSingle(value);
        }

        public static implicit operator float (SerializableSingle value)
        {
            return value.value;
        }
    }

    public class SerializableInt32 : Body
    {
        public int value;

        public SerializableInt32(int value)
        {
            this.value = value;
        }

        public SerializableInt32(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableInt32(int value)
        {
            return new SerializableInt32(value);
        }

        public static implicit operator int (SerializableInt32 value)
        {
            return value.value;
        }
    }

    public class SerializableInt64 : Body
    {
        public long value;

        public SerializableInt64(long value)
        {
            this.value = value;
        }

        public SerializableInt64(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableInt64(long value)
        {
            return new SerializableInt64(value);
        }

        public static implicit operator long (SerializableInt64 value)
        {
            return value.value;
        }
    }

    public class SerializableInt16 : Body
    {
        public short value;

        public SerializableInt16(short value)
        {
            this.value = value;
        }

        public SerializableInt16(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableInt16(short value)
        {
            return new SerializableInt16(value);
        }

        public static implicit operator short (SerializableInt16 value)
        {
            return value.value;
        }
    }

    public class SerializableUInt32 : Body
    {
        public uint value;

        public SerializableUInt32(uint value)
        {
            this.value = value;
        }

        public SerializableUInt32(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableUInt32(uint value)
        {
            return new SerializableUInt32(value);
        }

        public static implicit operator uint (SerializableUInt32 value)
        {
            return value.value;
        }
    }

    public class SerializableUInt64 : Body
    {
        public ulong value;

        public SerializableUInt64(ulong value)
        {
            this.value = value;
        }

        public SerializableUInt64(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableUInt64(ulong value)
        {
            return new SerializableUInt64(value);
        }

        public static implicit operator ulong (SerializableUInt64 value)
        {
            return value.value;
        }
    }

    public class SerializableUInt16 : Body
    {
        public ushort value;

        public SerializableUInt16(ushort value)
        {
            this.value = value;
        }

        public SerializableUInt16(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableUInt16(ushort value)
        {
            return new SerializableUInt16(value);
        }

        public static implicit operator ushort (SerializableUInt16 value)
        {
            return value.value;
        }
    }

    public class SerializableString : Body
    {
        public string value;

        public SerializableString(string value)
        {
            this.value = value;
        }

        public SerializableString(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableString(string value)
        {
            return new SerializableString(value);
        }

        public static implicit operator string (SerializableString value)
        {
            return value.value;
        }
    }

    public class SerializableVector3 : Body
    {
        public Vector3 value;

        public SerializableVector3(Vector3 value)
        {
            this.value = value;
        }

        public SerializableVector3(byte[] data)
            : base(data)
        { }

        public static implicit operator SerializableVector3(Vector3 value)
        {
            return new SerializableVector3(value);
        }

        public static implicit operator Vector3(SerializableVector3 value)
        {
            return value.value;
        }
    }

    public class SerializableTransform : Body
    {
        public string name;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public SerializableTransform(Transform value)
        {
            name = value.name;
            position = value.position;
            rotation = value.eulerAngles;
            scale = value.lossyScale;
        }

        public SerializableTransform(byte[] data)
            : base(data)
        { }

        public static implicit operator Transform(SerializableTransform value)
        {
            var tr = GameObject.Find(value.name).transform;
            tr.position = value.position;
            tr.eulerAngles = value.rotation;
            tr.localScale = value.scale;
            return tr;
        }
    }
}