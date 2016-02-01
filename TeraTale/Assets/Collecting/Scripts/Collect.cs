using UnityEngine;
using System.Collections;

public abstract class Collect : MonoBehaviour
{
    protected int _cnt;

    void Start()
    {
        _cnt = Random.Range(5,10);
        SetCreature();
    }

    protected abstract void SetCreature();

    protected abstract void GetSource();

    protected void GetCreature()
    {
        _cnt = Random.Range(5, 10);
        GetSource();
        Invoke("SetCreature", Random.Range(5, 10));
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "tool")
            _cnt--;

        if (_cnt <= 0)
            GetCreature();

        Debug.Log(_cnt);
    }
}
