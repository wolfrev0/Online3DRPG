using System;
using UnityEngine;
using System.Collections.Generic;
using TeraTaleNet;

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

    static Camera _npcCam;
    
    protected abstract List<Script> scripts { get; }
    public ItemStackList itemStacks = new ItemStackList(30);

    protected void Awake()
    {
        for (int i = 0; i < 30; i++)
            itemStacks.Add(new ItemStack());
        if (!_npcCam)
            _npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
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

        NPCDialog.instance.StartConversation(scripts);
        GlobalSound.instance.PlayNPCHello();
    }
}