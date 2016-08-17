using System.Collections.Generic;

public class Edan : NPC
{
    protected override List<Script> scripts
    {
        get
        {
            List<Script> _scripts = new List<Script>();

            Script s;
            Script.Command cmd;

            s.commands = new List<Script.Command>();
            s.comment = "으...으윽...";
            cmd.name = "나가기";
            cmd.action = () => { NPCDialog.instance.Close(false); };
            s.commands.Add(cmd);
            _scripts.Add(s);

            return _scripts;
        }
    }
}