using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Taylor : NPC
{
    protected new void Awake()
    {
        base.Awake();
        itemStacks[0].Push(new Pickaxe());
        itemStacks[1].Push(new HpPotion());
    }

    protected override List<Script> scripts
    {
        get
        {
            List<Script> _scripts = new List<Script>();

            Script s;
            Script.Command cmd;

            s.commands = new List<Script.Command>();
            s.comment = "안녕, 무슨일로 찾아왔니?";

            cmd.name = "대화";
            cmd.action = ()=>
            {
                s.commands = new List<Script.Command>();
                switch (Random.Range(0, 3))
                {
                    case 0:
                        s.comment = "우린 사이다 먹은 사이다? ㅎ하하하하핳";
                        break;
                    case 1:
                        s.comment = "사자를 사자! 크크킄ㅋㅋㅋ킄ㅋ";
                        break;
                    case 2:
                        s.comment = "판다를 어떻게 판다? 캬컄ㅋㅋ컄ㅋ";
                        break;
                }
                cmd.name = "Close";
                cmd.action = NPCDialog.instance.Close;
                s.commands.Add(cmd);
                _scripts.Add(s);

                NPCDialog.instance.Next();
            };
            s.commands.Add(cmd);

            cmd.name = "거래";
            cmd.action = ()=>
            {
                NPCShop.instance.Open(this);
                Inventory.instance.gameObject.SetActive(true);
                NPCDialog.instance.Close();
            };
            s.commands.Add(cmd);

            cmd.name = "퀘스트";
            cmd.action = () =>
            {
                s.commands = new List<Script.Command>();
                s.comment = "아직 퀘스트가 없어...";
                cmd.name = "Close";
                cmd.action = NPCDialog.instance.Close;
                s.commands.Add(cmd);
                _scripts.Add(s);

                NPCDialog.instance.Next();
            };
            s.commands.Add(cmd);

            _scripts.Add(s);

            return _scripts;
        }
    }
}