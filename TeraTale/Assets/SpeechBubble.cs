using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Text text;
    public Image image;
    public Image tail;

    public void Show(string chat)
    {
        gameObject.SetActive(true);

        text.text = chat;

        float width = text.preferredWidth > 35 ? text.preferredWidth : 35;
        float height = text.preferredHeight > 25 ? text.preferredHeight : 25;
        width = width < 188 ? width : 188;
        text.rectTransform.sizeDelta = new Vector2(width, height);
        image.rectTransform.sizeDelta = new Vector2(width + 13, height + 15);

        CancelInvoke("Hide");
        Invoke("Hide", 10);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
