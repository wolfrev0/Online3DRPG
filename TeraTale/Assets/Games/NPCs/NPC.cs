using System;
using UnityEngine;
using System.Collections.Generic;
using TeraTaleNet;
using UnityStandardAssets.ImageEffects;

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
    public bool helloSound = true;

    protected void Awake()
    {
        for (int i = 0; i < 30; i++)
            itemStacks.Add(new ItemStack());
        if (!_npcCam)
            _npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
    }

    public void StartConversation()
    {
        _npcCam.transform.position = transform.position + transform.forward * 2f + new Vector3(0, 1.4f, 0);
        _npcCam.transform.LookAt(transform);
        var angles = _npcCam.transform.eulerAngles;
        angles.x = 0;
        _npcCam.transform.eulerAngles = angles;
        _npcCam.enabled = true;
        _npcCam.depth = Camera.main.depth + 1;
        _npcCam.GetComponent<DepthOfFieldDeprecated>().objectFocus = transform;

        NPCDialog.instance.StartConversation(scripts);
        if (helloSound)
            GlobalSound.instance.PlayNPCHello();
    }
}