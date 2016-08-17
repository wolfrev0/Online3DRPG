using UnityEngine;

public class ItemSpawnEffector : MonoBehaviour
{
    public float xzAngle = 0;
    public float xzSpeed = 0;
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
            var y = (Mathf.Sin(_elapsed) - _prevSin);
            var xzDelta = xzSpeed * Time.deltaTime * 2;
            transform.position += new Vector3(Mathf.Sin(xzAngle) * xzDelta, y * 2f, Mathf.Cos(xzAngle) * xzDelta);
            _prevSin = Mathf.Sin(_elapsed);
            _elapsed += Time.deltaTime * _frequency;
        }
        else
        {
            _collider.enabled = true;
            enabled = false;
        }
    }
}