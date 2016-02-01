using UnityEngine;
using System.Collections;

public class aasdf : MonoBehaviour
{
	void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.forward * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(-Vector3.forward * Time.deltaTime);
	}
}
