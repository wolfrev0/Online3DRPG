using UnityEngine;
using System.Collections;
using System;

public class Lizard : KineticEntity
{
    protected override void Appear()
    {
        _animator.SetBool("Run", false);
                                                                                                                                                        
    }

    protected override void Disappear()
    {
        _animator.SetBool("Run", true);
    }

    void ReRun()
    {

    }
}
