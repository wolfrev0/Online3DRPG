using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class NPC : Entity
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
    public GameObject speechBubble;
    Camera npcCam;
    NPCDialog npcDialog;
    List<Script> _scripts = new List<Script>();

    protected void Awake()
    {
        npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
        npcDialog = FindObjectOfType<NPCDialog>();

        Script s;
        Script.Command cmd;

        s.commands = new List<Script.Command>();
        s.comment = "안녕";
        cmd.name = "Next";
        cmd.action = npcDialog.Next;
        s.commands.Add(cmd);
        _scripts.Add(s);

        s.commands = new List<Script.Command>();
        s.comment = "ㅎㅎㅎ";
        cmd.name = "Next";
        cmd.action = npcDialog.Next;
        s.commands.Add(cmd);
        _scripts.Add(s);

        s.commands = new List<Script.Command>();
        s.comment = "나는 오늘도 눈을 감고 음악을 듣는다... 음악만이 이 공간속에서 유일하게 허락된 마약이니깐, 크하하핫!!!";
        cmd.name = "Close";
        cmd.action = npcDialog.Close;
        s.commands.Add(cmd);
        _scripts.Add(s);
    }

    public void StartConversation()
    {
        npcCam.transform.position = transform.position + transform.forward + new Vector3(0, 1.58f, 0);
        npcCam.transform.LookAt(transform);
        var angles = npcCam.transform.eulerAngles;
        angles.x = 0;
        npcCam.transform.eulerAngles = angles;
        npcCam.enabled = true;
        npcCam.depth = Camera.main.depth + 1;

        npcDialog.StartConversation(_scripts);
    }
}