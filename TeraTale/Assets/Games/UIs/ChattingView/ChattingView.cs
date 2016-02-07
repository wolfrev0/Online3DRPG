using UnityEngine.UI;
using TeraTaleNet;
using UnityEngine.EventSystems;
using UnityEngine;

public class ChattingView : NetworkScript
{
    static public ChattingView instance;
    Text _text;

    protected void Awake()
    {
        instance = this;

        _text = GetComponent<Text>();
    }

    public void SendChat(string chat)
    {
        Send(new PushChat(chat));
    }

    void PushChat(PushChat info)
    {
        while (LayoutUtility.GetPreferredHeight(_text.rectTransform) > _text.rectTransform.rect.height)
        {
            _text.text = _text.text.Remove(0, _text.text.IndexOf('\n') + 1);
        }
        _text.text = _text.text + info.sender + " : " + info.chat + "\n";
        var speaker = Player.FindPlayerByName(info.sender);
        speaker.Speak(info.chat);
    }

    public void PushGuideMessage(string message)
    {
        while (LayoutUtility.GetPreferredHeight(_text.rectTransform) > _text.rectTransform.rect.height)
        {
            _text.text = _text.text.Remove(0, _text.text.IndexOf('\n') + 1);
        }
        _text.text = _text.text + message + "\n";
    }
}
