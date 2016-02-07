using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class NPC : MonoBehaviour
{
    public struct Script
    {
        public struct Command
        {
            public string name;
            public Action action;
        }
        public string comment;
        public List<Command> commands;
    }
    Camera _npcCam;
    protected NPCDialog npcDialog;
    protected abstract List<Script> scripts { get; }

    protected void Awake()
    {
        _npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
        npcDialog = FindObjectOfType<NPCDialog>();
    }

    public void StartConversation()
    {
        _npcCam.transform.position = transform.position + transform.forward + new Vector3(0, 1.58f, 0);
        _npcCam.transform.LookAt(transform);
        var angles = _npcCam.transform.eulerAngles;
        angles.x = 0;
        _npcCam.transform.eulerAngles = angles;
        _npcCam.enabled = true;
        _npcCam.depth = Camera.main.depth + 1;

        npcDialog.StartConversation(scripts);
    }
}