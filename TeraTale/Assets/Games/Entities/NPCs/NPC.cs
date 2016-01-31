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
        cmd.name = "Close";
        cmd.action = npcDialog.Close;
        s.commands.Add(cmd);
        _scripts.Add(s);

        s.commands = new List<Script.Command>();
        s.comment = "ㅎㅎㅎ";
        cmd.name = "Next";
        cmd.action = npcDialog.Next;
        s.commands.Add(cmd);
        cmd.name = "Close";
        cmd.action = npcDialog.Close;
        s.commands.Add(cmd);
        _scripts.Add(s);

        s.commands = new List<Script.Command>();
        s.comment = "잘가";
        cmd.name = "Next";
        cmd.action = npcDialog.Next;
        s.commands.Add(cmd);
        cmd.name = "Close";
        cmd.action = npcDialog.Close;
        s.commands.Add(cmd);
        _scripts.Add(s);
    }

    protected void Update()
    {
        if(Player.mine)
        {
            var ppos = Player.mine.transform.position;
            var npos = transform.position;
            if (Vector3.Distance(ppos, npos) < 3)
                speechBubble.gameObject.SetActive(true);
            else
                speechBubble.gameObject.SetActive(false);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    var ppos = Player.mine.transform.position;
                    var npos = transform.position;
                    if (Vector3.Distance(ppos, npos) < 3)
                        StartConversation();
                }
            }
        }
    }

    void StartConversation()
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