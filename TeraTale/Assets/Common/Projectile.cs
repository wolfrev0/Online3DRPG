using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public float speed = 1;
    public float autoDestroyTime = float.MaxValue;

    void Start()
    {
        transform.LookAt(direction + transform.position);
        Destroy(gameObject, autoDestroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Enemy" || coll.tag == "Terrain")
            Destroy(gameObject);
    }
}