using UnityEngine;
using System.Collections;

public class CreditDialog : MonoBehaviour
{
    public GameObject _credit = null;

    public void OnCredit()
    {
        _credit.SetActive(true);
    }

    public void OffCredit()
    {
        _credit.SetActive(false);
    }
}
