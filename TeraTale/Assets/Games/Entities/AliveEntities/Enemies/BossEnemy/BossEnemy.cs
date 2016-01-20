using UnityEngine;
using System.Collections;
using sunho;

public abstract class BossEnemy : Enemy2
{
    protected abstract void OnFirstPatten();
    protected abstract void OnSecondPatten();
    protected abstract void OnThirdPatten();
}
