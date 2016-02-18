using System.Collections.Generic;

public class Ian : NPC
{
    protected override List<Script> scripts
    {
        get
        {
            List<Script> _scripts = new List<Script>();

            Script s;
            Script.Command cmd;

            s.commands = new List<Script.Command>();
            s.comment = "내 안의 흑염룡이 날뛰는군. 크크큭...";
            cmd.name = "Next";
            cmd.action = NPCDialog.instance.Next;
            s.commands.Add(cmd);
            _scripts.Add(s);

            s.commands = new List<Script.Command>();
            s.comment = "그것은 마치... 운명의 데스티니, 죽음의 데스!";
            cmd.name = "Next";
            cmd.action = NPCDialog.instance.Next;
            s.commands.Add(cmd);
            _scripts.Add(s);

            s.commands = new List<Script.Command>();
            s.comment = "나는 오늘도 눈을 감고 음악을 듣는다... 음악만이 이 공간속에서 유일하게 허락된 마약이니깐, 크하하핫!!!";
            cmd.name = "Close";
            cmd.action = () => { NPCDialog.instance.Close(true); };
            s.commands.Add(cmd);
            _scripts.Add(s);

            return _scripts;
        }
    }
}