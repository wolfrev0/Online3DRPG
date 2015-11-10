using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform _target;
    
    void Update()
    {
        if (_target != null)
        {
            transform.position = _target.transform.position + new Vector3(0, 8, -8);
        }
    }
}