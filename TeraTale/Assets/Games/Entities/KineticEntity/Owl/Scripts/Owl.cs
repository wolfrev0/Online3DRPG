using UnityEngine;
using System.Collections;
using System;

public class Owl : KineticEntity
{
    protected override void Appear()
    {
        _animator.SetBool("Fly", false);
    }

    protected override void Disappear()
    {
        _animator.SetBool("Fly", true);
    }
}
