using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    public void JoinCustomize()
    {
        SceneManager.LoadScene("Customizing");
    }

    public void JoinLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void Quit()
    { Application.Quit(); }
}
