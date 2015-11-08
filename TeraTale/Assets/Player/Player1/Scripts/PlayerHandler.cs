using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
    const float _kRaycastDistance = 50.0f;
    PlayerController _playerController;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        Debug.Log(123);
        if (Input.GetButtonDown("Move"))
        {
            Debug.Log(456);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, _kRaycastDistance))
            {
                _playerController.destination = hit.point;
            }
        }
    }
}