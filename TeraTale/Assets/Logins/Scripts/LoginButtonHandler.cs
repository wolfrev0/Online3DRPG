using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    public LoginManager loginManager;
    public InputField id;
    public InputField pw;

    public void OnButtonClicked()
    {
        loginManager.SendLoginRequest(id.text, pw.text);
    }
}