using UnityEngine.UI;
using TeraTaleNet;

public class ChattingView : NetworkScript
{
    Text _text;

    void Awake()
    {
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
}
