using UnityEngine;
using System.Collections;

public class SummonMeteor : MonoBehaviour
{
    public GameObject _meteor;

    void MagicCircle()
    {
        GameObject obj = (GameObject)Instantiate(_meteor);
        obj.transform.position = this.transform.position;
        obj.transform.localScale = Vector3.one;
    }

    void DestroyMagicCircle()
    {
        Destroy(this.gameObject);
    }
}
