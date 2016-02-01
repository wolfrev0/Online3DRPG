using UnityEngine;
using System.Collections;
using System;

public class CollectFlower : Collect
{
    protected override void SetCreature()
    {
        Debug.Log("SetCreature");
    }

    protected override void GetSource()
    {
        Debug.Log("GetSource");
    }
}
