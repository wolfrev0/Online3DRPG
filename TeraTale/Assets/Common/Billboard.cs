using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    
	void LateUpdate ()
    {
        if (target == null)
            target = GameObject.FindWithTag("MainCamera").transform;
        transform.LookAt(target.position);
        transform.Rotate(offset);
	}
}
