using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    LoginManager loginManager;

    void Start()
    {
        loginManager = FindObjectOfType<LoginManager>();
    }

    public void OnButtonClicked()
    {
        loginManager.SendLoginRequest(id.text, pw.text);
    }
}