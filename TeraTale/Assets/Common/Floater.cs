using UnityEngine;

public class Floater : MonoBehaviour
{
    public float amplitude = 1;
    public float frequency = 1;
    public float rotationSpeed = 1;
    float _elapsed = 0;
    float _prevSin;

    void Start()
    {
        _prevSin = Mathf.Sin(_elapsed * frequency);
    }

    void Update()
    {
        _elapsed += Time.deltaTime * frequency;
        var y = (Mathf.Sin(_elapsed) - _prevSin) * amplitude;
        transform.position += new Vector3(0, y, 0);
        _prevSin = Mathf.Sin(_elapsed);

        transform.Rotate(0, rotationSpeed * Time.deltaTime * 60, 0, Space.World);
    }
}
