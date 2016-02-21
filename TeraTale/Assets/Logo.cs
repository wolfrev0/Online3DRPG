using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    void Awake()
    {
        Invoke("LoadLogin", 3);
    }

    void LoadLogin()
    {
        SceneManager.LoadScene("Login");
    }
}