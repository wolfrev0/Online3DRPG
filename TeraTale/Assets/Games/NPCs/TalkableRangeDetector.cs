using UnityEngine;
using UnityEngine.UI;

public class TalkableRangeDetector : MonoBehaviour
{
    public Button dialogStart;

    void OnTriggerEnter(Collider coll)
    {
        if (Player.mine && coll.gameObject == Player.mine.gameObject)
            dialogStart.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider coll)
    {
        if (Player.mine && coll.gameObject == Player.mine.gameObject)
            dialogStart.gameObject.SetActive(false);
    }
}