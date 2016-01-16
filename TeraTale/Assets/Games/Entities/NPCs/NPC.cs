using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class NPC : Entity
{
    Camera npcCam;
    NPCDialog npcDialog;

    protected void Awake()
    {
        npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
        npcDialog = FindObjectOfType<NPCDialog>();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    npcCam.transform.position = transform.position + transform.forward + new Vector3(0, 1.58f, 0);
                    npcCam.transform.LookAt(transform);
                    var angles = npcCam.transform.eulerAngles;
                    angles.x = 0;
                    npcCam.transform.eulerAngles = angles;
                    npcCam.enabled = true;
                    npcCam.depth = Camera.main.depth + 1;
                    npcDialog.text.text = "내 안의 흑염룡이 날뛰는군. 크크큭...";
                }
            }
        }
    }


}