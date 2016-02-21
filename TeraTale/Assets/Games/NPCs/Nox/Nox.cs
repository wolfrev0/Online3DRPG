using System.Collections.Generic;
using TeraTaleNet;

public class Nox : NPC
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
            cmd.name = "퀘스트";
            cmd.action = () =>
            {
                if (Player.mine.gotQuest)
                {
                    if(Player.mine.ItemCount(new Apple()) >= 3)
                    {
                        Player.mine.RemoveItem(new Apple(), 3);
                        for (int i = 0; i < 2; i++)
                            Player.mine.AddItem(new HpPotion());
                        Player.mine.ExpUp(new ExpUp(50));
                        Player.mine.RemoveQuest();

                        s.commands = new List<Script.Command>();
                        s.comment = "사과 3개를 전부 회수해왔구나. 정말 고마워! 선물로 포션 2개와 경험치 50을 보냈어.";
                        cmd.name = "나가기";
                        cmd.action = () => { NPCDialog.instance.Close(true); };
                        s.commands.Add(cmd);
                        _scripts.Add(s);

                        NPCDialog.instance.Next();
                    }
                    else
                    {
                        s.commands = new List<Script.Command>();
                        s.comment = "좀비들이 훔쳐간 사과 3개를 아직 전부 회수하지 못했구나, 걱정되네 ㅠㅠ";
                        cmd.name = "나가기";
                        cmd.action = () => { NPCDialog.instance.Close(true); };
                        s.commands.Add(cmd);
                        _scripts.Add(s);

                        NPCDialog.instance.Next();
                    }
                }
                else
                {
                    s.commands = new List<Script.Command>();
                    s.comment = "요즘 우리 과수원에 좀비들이 자주 출몰해서 사과농사를 망치고있어.\n좀비들이 훔쳐간 사과 3개를 되찾아와 주겠니?\n보상으로 포션 2개와 경험치 50을 줄게.";
                    cmd.name = "수락";
                    cmd.action = () =>
                    {
                        Player.mine.AddQuest();

                        s.commands = new List<Script.Command>();
                        s.comment = "그럼 꼭 좀 부탁할게, 고마워!";
                        cmd.name = "나가기";
                        cmd.action = () => { NPCDialog.instance.Close(true); };
                        s.commands.Add(cmd);
                        _scripts.Add(s);

                        NPCDialog.instance.Next();
                    };
                    s.commands.Add(cmd);
                    cmd.name = "거절";
                    cmd.action = () =>
                    {
                        s.commands = new List<Script.Command>();
                        s.comment = "그래? 아쉽지만 다른친구에게 부탁해봐야겠네 으으...";
                        cmd.name = "나가기";
                        cmd.action = () => { NPCDialog.instance.Close(true); };
                        s.commands.Add(cmd);
                        _scripts.Add(s);

                        NPCDialog.instance.Next();
                    };
                    s.commands.Add(cmd);
                    _scripts.Add(s);

                    NPCDialog.instance.Next();
                }
            };
            s.commands.Add(cmd);
            _scripts.Add(s);

            return _scripts;
        }
    }
}