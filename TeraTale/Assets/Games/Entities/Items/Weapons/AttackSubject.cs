using UnityEngine;

public abstract class AttackSubject : MonoBehaviour
{
    public AliveEntity owner;
    public string targetTag;
}