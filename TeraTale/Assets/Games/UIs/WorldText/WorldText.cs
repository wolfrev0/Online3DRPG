using UnityEngine;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
    public float life = 1;
    public float speed = 1;
    Text _text;
    public string text { get { return _text.text; } set { _text.text = value; } }
    public Color color { get { return _text.color; } set { _text.color = value; } }

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Start()
    {
        Destroy(gameObject, life);
    }

    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}