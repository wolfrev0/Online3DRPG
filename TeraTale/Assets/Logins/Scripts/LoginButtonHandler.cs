using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    Certificator loginManager;

    void Start()
    {
        loginManager = FindObjectOfType<Certificator>();
    }

    public void OnButtonClicked()
    {
        loginManager.SendLoginRequest(id.text, pw.text);
    }
}