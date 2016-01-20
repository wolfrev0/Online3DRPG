using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public Button pfButton;
    public Text text;
    public Transform menu;
    Camera npcCam;
    List<NPC.Script> _scripts;
    int _nextScriptIdx = 0;

    void Awake()
    {
        npcCam = GameObject.FindWithTag("NPCCamera").GetComponent<Camera>();
    }

    public void StartConversation(List<NPC.Script> scripts)
    {
        _scripts = scripts;
        _nextScriptIdx = 0;
        Next();
    }

    public void Next()
    {
        if(_nextScriptIdx < _scripts.Count)
        {
            var script = _scripts[_nextScriptIdx++];
            text.text = script.comment;

            foreach (Transform trChild in menu)
                Destroy(trChild.gameObject);
            foreach (var cmd in script.commands)
            {
                var button = Instantiate(pfButton);
                button.transform.SetParent(menu.transform, false);
                button.GetComponentInChildren<Text>().text = cmd.name;
                button.onClick.AddListener(new UnityAction(cmd.action));
            }
        }
    }

    public void Close()
    {
        npcCam.enabled = false;
        npcCam.depth = Camera.main.depth - 1;
    }
}
