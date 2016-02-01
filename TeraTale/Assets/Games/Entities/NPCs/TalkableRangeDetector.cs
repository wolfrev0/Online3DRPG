using UnityEngine;
using UnityEngine.UI;

public class TalkableRangeDetector : MonoBehaviour
{
    public Button dialogStart;

    void OnTriggerEnter(Collider coll)
    {
        dialogStart.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider coll)
    {
        dialogStart.gameObject.SetActive(false);
    }
}