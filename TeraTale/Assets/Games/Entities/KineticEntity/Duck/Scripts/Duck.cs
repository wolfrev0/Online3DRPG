using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        Invoke("Eat", Random.Range(1, 3));
    }

    void Eat()
    {
        _animator.SetBool("Eat", true);
        Invoke("Eat", Random.Range(7, 15));
       
    }

    void EatEnd()
    {
        _animator.SetBool("Eat", false);
    }
}
