using UnityEngine;

public class DestinationSign : MonoBehaviour
{
    void Update()
    {
        transform.localScale -= Vector3.one * 0.04f;
        if (transform.localScale.x < 0.4f)
            transform.localScale = Vector3.one * 0.4f;

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse1) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, LayerMask.GetMask("Terrain")))
        {
            transform.position = hit.point + Vector3.up * 0.01f;
            transform.localScale = Vector3.one;
        }
    }
}