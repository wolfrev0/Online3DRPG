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
            s.comment = "안녕하십니까. 어디 불편한데라도 있으신지?";
            cmd.name = "대화하기";
            cmd.action = () =>
            {
                s.commands = new List<Script.Command>();
                s.comment = "요즘 부상자가 늘어서 힘드네...에고";
                cmd.name = "나가기";
                cmd.action = () => { NPCDialog.instance.Close(true); };
                s.commands.Add(cmd);
                _scripts.Add(s);

                NPCDialog.instance.Next();
            };
            s.commands.Add(cmd);
            cmd.name = "치료받기";
            cmd.action = () =>
            {
                Player.mine.Heal(new TeraTaleNet.Heal("", Player.mine.hpMax));
                //stamina heal not implemented

                s.commands = new List<Script.Command>();
                s.comment = "치료가 완료되었습니다. 좋은하루 되세요~ ^^*";
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