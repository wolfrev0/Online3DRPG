using System.Collections.Generic;

public class Hwang : NPC
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
                s.comment = "음, 딱히 할 말이 없네.";
                cmd.name = "나가기";
                cmd.action = () => { NPCDialog.instance.Close(true); };
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