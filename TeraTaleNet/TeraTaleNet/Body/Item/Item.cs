using System;
using System.Reflection;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class Item : Body
    {
        static int currentItemID = 0;

        public int _itemID = currentItemID++;

        public Sprite sprite
        { get { return Resources.Load<Sprite>("Textures/" + GetType().Name); } }

        public GameObject solidPrefab
        { get { return Resources.Load<GameObject>("Prefabs/" + GetType().Name); } }

        public Item clone
        {
            get
            {
                var clone = Activator.CreateInstance(GetType());

                FieldInfo[] fi = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                for (int i = 0; i < fi.Length; i++)
                    fi[i].SetValue(clone, fi[i].GetValue(this));

                return (Item)clone;
            }
        }

        public abstract int maxCount { get; }
        public bool isConsumables { get { return GetType().IsSubclassOf(typeof(Consumable)); } }
        public virtual bool isNull { get { return false; } }

        public Item()
        { }

        public virtual void Use() { }

        public bool IsSameType(Item other)
        { return GetType() == other.GetType(); }

        public static bool operator ==(Item a, Item b)
        {
            var nullA = ReferenceEquals(null, a);
            var nullB = ReferenceEquals(null, b);
            if (nullA && nullB)
                return true;
            if (nullA || nullB)
                return false;
            return a._itemID == b._itemID;
        }
        public static bool operator !=(Item a, Item b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && this == (Item)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 397 ^ _itemID.GetHashCode();
            }
        }
    }
}