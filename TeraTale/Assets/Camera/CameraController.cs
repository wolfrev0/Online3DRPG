using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform _target;
    Vector3 relativeAtTargetPos = new Vector3(0, 6, -6);

    void Update()
    {
        if (_target != null)
        {
            transform.position = _target.transform.position + relativeAtTargetPos;

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