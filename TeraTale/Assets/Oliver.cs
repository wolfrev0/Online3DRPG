using System.Collections.Generic;

public class Oliver : NPC
{
    protected override List<Script> scripts
    {
        get
        {
            List<Script> _scripts = new List<Script>();

            Script s;
            Script.Command cmd;

            s.commands = new List<Script.Command>();
            s.comment = "으흐흑... 내 집.. 내 가족...\n도와주세요 으흑...";
            cmd.name = "나가기";
            cmd.action = () => { NPCDialog.instance.Close(false); };
            s.commands.Add(cmd);
            _scripts.Add(s);

            return _scripts;
        }
    }
}