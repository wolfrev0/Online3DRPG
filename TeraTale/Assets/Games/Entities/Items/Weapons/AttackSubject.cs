using System;
using UnityEngine;

public abstract class AttackSubject : MonoBehaviour
{
    public AliveEntity owner { get; set; }
    public Func<float, float> damageCalculator = (float original)=> { return original; };
    public string targetTag;
    public bool knockdown = false;
}