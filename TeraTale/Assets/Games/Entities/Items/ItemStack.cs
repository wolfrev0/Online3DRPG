using UnityEngine;
using TeraTaleNet;
using System;

public class ItemStackOverflow : Exception
{ }

public class ItemTypeMismatch : Exception
{ }

public class ItemStack
{
    Item _item = new ItemNull();
    int _count = 0;

    public int count { get { return _count; } }
    public Sprite sprite { get { return _item.sprite; } }
    public Item item { get { return _item; } }

    public ItemStack()
    { }

    public void Push(Item item)
    {
        if (_item.isNull)
            _item = item;

        if (_item.IsSameType(item))
        {
            if (IsFull() == false)
                _count++;
            else
                throw new ItemStackOverflow();
        }
        else
            throw new ItemTypeMismatch();
    }

    bool IsFull()
    {
        return _item.maxCount == _count;
    }

    public bool IsPushable(Item item)
    {
        return _item.isNull || (IsFull() == false && _item.IsSameType(item));
    }

    public void Use()
    {
        _item.Use();

        if (_item.isConsumables)
        {
            _count--;
            if (count <= 0)
                _item = new ItemNull();
        }
    }
}