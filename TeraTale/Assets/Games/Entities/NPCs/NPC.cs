using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
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
    public Transform[] _trans;
    public GameObject speechBubble;
    float _fDelay;
    Camera npcCam;
    NPCDialog npcDialog;
    List<Script> _scripts = new List<Script>();
    NavMeshAgent _nav;
    Animator _animator;
    bool bChk;

    protected void Awake()
    {
        npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
        npcDialog = FindObjectOfType<NPCDialog>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _fDelay = Time.time; 

        Script s;
        Script.Command cmd;

        //s.commands = new List<Script.Command>();
        //s.comment = "안녕";
        //cmd.name = "Next";
        //cmd.action = npcDialog.Next;
        //s.commands.Add(cmd);
        //cmd.name = "Close";
        //cmd.action = npcDialog.Close;
        //s.commands.Add(cmd);
        //_scripts.Add(s);

        //s.commands = new List<Script.Command>();
        //s.comment = "ㅎㅎㅎ";
        //cmd.name = "Next";
        //cmd.action = npcDialog.Next;
        //s.commands.Add(cmd);
        //cmd.name = "Close";
        //cmd.action = npcDialog.Close;
        //s.commands.Add(cmd);
        //_scripts.Add(s);

        //s.commands = new List<Script.Command>();
        //s.comment = "잘가";
        //cmd.name = "Next";
        //cmd.action = npcDialog.Next;
        //s.commands.Add(cmd);
        //cmd.name = "Close";
        //cmd.action = npcDialog.Close;
        //s.commands.Add(cmd);
        //_scripts.Add(s);

        Invoke("Around", 2);
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

    void Around()
    {
        if (_nav.velocity.magnitude == 0f && bChk)
        {   
            _nav.SetDestination(_trans[0].position);

            bChk = !bChk;
        }
        else if(_nav.velocity.magnitude == 0f && !bChk)
        {
            _nav.SetDestination(_trans[1].position);

            bChk = !bChk;
        }

        _animator.SetBool("Walk", true);
        Invoke("Return", UnityEngine.Random.Range(3, 5));

        Debug.Log(bChk.ToString());
    }

    void Return()
    {
        _animator.SetBool("Walk", false);

        transform.localEulerAngles = new Vector3(0, 0, 0);

        Invoke("Around", UnityEngine.Random.Range(3, 5));
    }
}