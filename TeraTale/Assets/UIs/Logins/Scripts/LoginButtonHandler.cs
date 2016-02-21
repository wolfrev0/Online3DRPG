using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    Certificator _certificator;

    void Awake()
    {
        _certificator = FindObjectOfType<Certificator>();
    }

    public void OnButtonClicked()
    {
        _certificator.SendLoginRequest(id.text, pw.text);
    }

    public void OnCustomizing()
    {
        SceneManager.LoadScene("Customizing");
    }
}