using UnityEngine;
using System.Collections;
using System;

public class Bird : KineticEntity
{
    public Texture[] _birdTexture;
 
    protected override void Appear()
    {
        _animator.SetBool("Idle", true);
        _skimesh.material.mainTexture = _birdTexture[UnityEngine.Random.Range(0, 4)];
    }

    protected override void Disappear()
    {
        _animator.SetBool("Idle", false);
    }
}
