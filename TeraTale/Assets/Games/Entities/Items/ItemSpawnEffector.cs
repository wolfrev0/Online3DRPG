using UnityEngine;
using System.Collections;

public class ItemSpawnEffector : MonoBehaviour
{
    float _time = 1.5f;
    float _elapsed;
    float _frequency;
    float _prevSin;
    Collider _collider;

    void Start()
    {
        _frequency = Mathf.PI * 2 * 0.5f / _time;
        _prevSin = Mathf.Sin(_elapsed * _frequency);
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    void Update()
    {
        if (_elapsed / _frequency < _time)
        {
            _elapsed += Time.deltaTime * _frequency;
            var y = (Mathf.Sin(_elapsed) - _prevSin);
            transform.position += new Vector3(0, y * 2f, 0);
            _prevSin = Mathf.Sin(_elapsed);
        }
        else
        {
            _collider.enabled = true;
            enabled = false;
        }
    }
}