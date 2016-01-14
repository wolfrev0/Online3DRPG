using System;
using System.Reflection;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class Item : Body
    {
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

        public Item()
        { }

        public Item(byte[] data)
            : base(data)
        { }

        public virtual void Use() { }

        public bool IsSameType(Item other)
        {
            return GetType() == other.GetType();
        }
    }
}