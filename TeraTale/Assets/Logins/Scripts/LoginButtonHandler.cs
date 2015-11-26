using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    Certificator _certificator;

    void Start()
    {
        _certificator = FindObjectOfType<Certificator>();
    }

    public void OnButtonClicked()
    {
        _certificator.SendLoginRequest(id.text, pw.text);
    }
}