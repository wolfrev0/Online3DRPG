using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public float speed = 1;
    public float autoDestroyTime = float.MaxValue;

    protected void Start()
    {
        transform.LookAt(direction + transform.position);
        Destroy(gameObject, autoDestroyTime);
    }

    protected void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Terrain")
            Destroy(gameObject);
    }
}