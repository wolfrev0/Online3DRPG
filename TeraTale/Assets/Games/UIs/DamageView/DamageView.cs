using UnityEngine;
using UnityEngine.UI;

public class DamageView : MonoBehaviour
{
    public float life = 1;
    public float speed = 1;
    Text _text;

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

    public void SetDamage(float amount)
    {
        _text.text = amount.ToString();
    }
}