using UnityEngine;
using UnityEngine.UI;

public class TalkableRangeDetector : MonoBehaviour
{
    public Button dialogStart;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject == Player.mine.gameObject)
            dialogStart.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject == Player.mine.gameObject)
            dialogStart.gameObject.SetActive(false);
    }
}