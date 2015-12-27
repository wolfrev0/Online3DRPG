using UnityEngine;
using UnityEngine.UI;

public class ChattingView : MonoBehaviour
{
    Text _text;

	void Start ()
    {
        _text = GetComponent<Text>();
	}

    public void PushChat(string chat)
    {
        if (LayoutUtility.GetPreferredHeight(_text.rectTransform) > _text.rectTransform.rect.height)
        {
            _text.text = _text.text.Remove(0, _text.text.IndexOf('\n') + 1);
        }
        _text.text += chat += "\n";
    }
}
