using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 relativeAtTargetPos = new Vector3(0, 8, -8);

    void Update()
    {
        if (target != null)
        {
            transform.position = target.transform.position + relativeAtTargetPos;
            
            if (3 > relativeAtTargetPos.magnitude)
            {
                relativeAtTargetPos = relativeAtTargetPos.normalized * 3;
            }
            else if (11 < relativeAtTargetPos.magnitude)
            {
                relativeAtTargetPos = relativeAtTargetPos.normalized * 11;
            }
            else
            {
                float wheelScroll = Input.GetAxis("Mouse ScrollWheel"); 
                relativeAtTargetPos += new Vector3(0, -wheelScroll, wheelScroll);
            }
        }
    }
}