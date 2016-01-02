using UnityEngine;
using UnityEngine.UI;
using TeraTaleNet;

public class ChattingView : MonoBehaviour
{
 //   Text _text;

	//void Start ()
 //   {
 //       _text = GetComponent<Text>();
	//}

 //   public void PushChat(string chat)
 //   {
 //       if (LayoutUtility.GetPreferredHeight(_text.rectTransform) > _text.rectTransform.rect.height)
 //       {
 //           _text.text = _text.text.Remove(0, _text.text.IndexOf('\n') + 1);
 //       }
 //       _text.text += chat += "\n";
 //   }
    NetworkSignaller _net;
    Text _text;

    void Start()
    {
        _net = GetComponent<NetworkSignaller>();
        _text = GetComponent<Text>();
    }

    public void SendChat(string chat)
    {
        _net.SendRPC(new PushChat(RPCType.AllBuffered, chat));
    }

    void PushChat(PushChat info)
    {
        if (LayoutUtility.GetPreferredHeight(_text.rectTransform) > _text.rectTransform.rect.height)
        {
            _text.text = _text.text.Remove(0, _text.text.IndexOf('\n') + 1);
        }
        _text.text = _text.text + info.sender + " : " + info.chat + "\n";
    }
}
