using System.Collections.Generic;
using TeraTaleNet;

public class Lobo : NPC
{
    protected override List<Script> scripts
    {
        get
        {
            List<Script> _scripts = new List<Script>();

            Script s;
            Script.Command cmd;

            s.commands = new List<Script.Command>();
            s.comment = "안녕, 무슨일이니?";
            cmd.name = "대화하기";
            cmd.action = () =>
            {
                s.commands = new List<Script.Command>();
                s.comment = "수인 좋아하니?";
                cmd.name = "응";
                cmd.action = () =>
                {
                    Player.mine.AddItem(new HpPotion());

                    s.commands = new List<Script.Command>();
                    s.comment = "그래, 착한녀석이구만! 선물로 포션하나 보냈어.";
                    cmd.name = "나가기";
                    cmd.action = () => { NPCDialog.instance.Close(true); };
                    s.commands.Add(cmd);
                    _scripts.Add(s);

                    NPCDialog.instance.Next();
                };
                s.commands.Add(cmd);
                cmd.name = "아니";
                cmd.action = () =>
                {
                    s.commands = new List<Script.Command>();
                    s.comment = "흥!";
                    cmd.name = "나가기";
                    cmd.action = () => { NPCDialog.instance.Close(true); };
                    s.commands.Add(cmd);
                    _scripts.Add(s);

                    NPCDialog.instance.Next();
                };
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