using UnityEngine;
using TeraTaleNet;
using System;
using System.Collections.Generic;

public class ItemStackOverflow : Exception
{ }

public class ItemTypeMismatch : Exception
{ }

public class ItemStack
{
    Stack<Item> _stack = new Stack<Item>(new[] { new ItemNull() });

    public int count { get { return _stack.Count; } }
    public Sprite sprite { get { return _stack.Peek().sprite; } }
    public Item item { get { return _stack.Peek(); } }

    public ItemStack()
    { }

    public void Push(Item item)
    {
        Item i = _stack.Peek();
        if (i.isNull || i.IsSameType(item))
        {
            if (IsFull() == false)
            {
                if (i.isNull)
                    _stack.Pop();
                _stack.Push(item);
            }
            else
                throw new ItemStackOverflow();
        }
        else
            throw new ItemTypeMismatch();
    }

    bool IsFull()
    {
        return _stack.Peek().maxCount <= _stack.Count;
    }

    public bool IsPushable(Item item)
    {
        Item i = _stack.Peek();

        if (i.isNull)
            return true;
        return IsFull() == false && i.IsSameType(item);
    }

    public void Use()
    {
        Item item = _stack.Peek();
        item.Use();

        if (item.isConsumables)
        {
            _stack.Pop();
            if (_stack.Count == 0)
                _stack.Push(new ItemNull());
        }
    }
}