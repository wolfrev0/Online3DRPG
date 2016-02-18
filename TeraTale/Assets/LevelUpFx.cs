using UnityEngine;
using UnityEngine.UI;

public class LevelUpFx : MonoBehaviour
{
    Image img;
    float elapsed;

    void Start()
    {
        Invoke("Remove", 2);
        img = GetComponent<Image>();
    }

    void Update()
    {
        transform.position += new Vector3(0, Time.deltaTime, 0);
        if (elapsed > 1)
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - Time.deltaTime);
        elapsed += Time.deltaTime;
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}