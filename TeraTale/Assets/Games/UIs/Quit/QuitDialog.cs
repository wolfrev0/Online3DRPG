using UnityEngine;
using System.Collections;

public class QuitDialog : MonoBehaviour
{
    public GameObject _quitPopup;

   public void ShowQuit()
    {
        _quitPopup.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OffQuit()
    {
        _quitPopup.SetActive(false);
    }
}
