using UnityEngine;

public class Floater : MonoBehaviour
{
    public float amplitude = 1;
    public float frequency = 1;
    public float rotationSpeed = 1;
    float elapsed = 0;
    float prevSin;

    void Start()
    {
        prevSin = Mathf.Sin(elapsed * frequency);
    }

    void Update()
    {
        elapsed += Time.deltaTime * frequency;
        var y = (Mathf.Sin(elapsed) - prevSin) * amplitude;
        transform.position += new Vector3(0, y, 0);
        prevSin = Mathf.Sin(elapsed);

        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }
}
