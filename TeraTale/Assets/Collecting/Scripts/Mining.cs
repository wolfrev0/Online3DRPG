using UnityEngine;
using System.Collections;

public class Mining : Collect
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
