using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    Certificator loginManager;
    public InputField id;
    public InputField pw;

    void Start()
    {
        loginManager = FindObjectOfType<Certificator>();
    }

    public void OnButtonClicked()
    {
        loginManager.SendLoginRequest(id.text, pw.text);
    }
}