using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    public Sprite[] sprites;
    public Image loadingBar;
    public Text loadingText;
    Image image;
    Vector2 _destination;
    float _elapsed = 0;
    const float loadingDelay = 1;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = sprites[Random.Range(0, sprites.Length)];
        Invoke("Close", loadingDelay);
    }

    void Update()
    {
        _elapsed += Time.deltaTime;
        image.rectTransform.anchoredPosition *= 9;
        image.rectTransform.anchoredPosition += _destination;
        image.rectTransform.anchoredPosition /= 10;
        loadingBar.fillAmount = _elapsed / loadingDelay;
        loadingText.text = string.Format("{0:0}%", _elapsed / loadingDelay * 100); ;
    }

    void Close()
    {
        _destination = Vector2.up * 900;
        Invoke("Remove", 2);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}