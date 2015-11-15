using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour {

    public InputField id;
    public InputField pw;

    public void OnButtonClicked()
    {
        LoginManager.instance.SendLoginRequest(id.text, pw.text);
    }
}