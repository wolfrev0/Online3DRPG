using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class Item : Body
    {
        public enum Type
        {
            none,
            sundry,
            consumable,
            equipment,
        }

        static int currentItemID = 0;

        public int _itemID = currentItemID++;
        
        public string name { get { return GetType().Name; } }
        public abstract int price { get; }
        public abstract Type itemType { get; }
        public abstract string effectExplanation { get; }
        public abstract string explanation { get; }

        public Sprite sprite
        { get { return Resources.Load<Sprite>("Textures/" + name); } }

        public GameObject solidPrefab
        { get { return Resources.Load<GameObject>("Prefabs/" + name); } }

        public Item clone
        {
            get
            {
                Item clone = (Item)Activator.CreateInstance(GetType());

                int savedID = clone._itemID;
                FieldInfo[] fi = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                for (int i = 0; i < fi.Length; i++)
                    fi[i].SetValue(clone, fi[i].GetValue(this));
                clone._itemID = savedID;

                return clone;
            }
        }

        public abstract int maxCount { get; }
        public bool isConsumables { get { return GetType().IsSubclassOf(typeof(Consumable)); } }
        public bool isNull { get { return itemType == Type.none; } }

        static Item()
        {
            try
            {
                currentItemID = Serializer.ToInt32(File.ReadAllBytes(@"C:\Users\Lobo\Desktop\Projects\TeraTale\TeraTale\ServerStates\ItemID"), 0);
            }
            catch (IOException)
            { }
        }

        static public void Save()
        {
            File.WriteAllBytes(@"C:\Users\Lobo\Desktop\Projects\TeraTale\TeraTale\ServerStates\ItemID", Serializer.Serialize(currentItemID));
        }

        public Item()
        { }

        public virtual void Use(object player) { }

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