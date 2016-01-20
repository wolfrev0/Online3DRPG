using UnityEngine;
using System.Collections;

public class ItemSpawnEffector : MonoBehaviour
{
    public float seed = Random.Range(0, 2 * Mathf.PI);
    public float xzSpeed = Random.Range(0f, 1f);
    float _time = 1.5f;
    float _elapsed;
    float _frequency;
    float _prevSin;
    Collider _collider;
    Vector3 _xzDir;

    void Start()
    {
        _frequency = Mathf.PI * 2 * 0.5f / _time;
        _prevSin = Mathf.Sin(_elapsed * _frequency);
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _xzDir = new Vector3(Mathf.Sin(seed), 0, Mathf.Cos(seed)) * 2;
    }

    void Update()
    {
        if (_elapsed / _frequency < _time)
        {
            _elapsed += Time.deltaTime * _frequency;
            var y = (Mathf.Sin(_elapsed) - _prevSin);
            transform.position += new Vector3(0, y * 2f, 0)/* + _xzDir * xzSpeed / (60 * _time)*/;
            _prevSin = Mathf.Sin(_elapsed);
        }
        else
        {
            _collider.enabled = true;
            enabled = false;
        }
    }
}