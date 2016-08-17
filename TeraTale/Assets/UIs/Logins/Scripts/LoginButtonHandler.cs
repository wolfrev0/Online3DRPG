using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TeraTaleNet;

public class LoginButtonHandler : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    public Text noticeView;
    Certificator _certificator;

    void Awake()
    {
        _certificator = FindObjectOfType<Certificator>();
        _certificator.onLoginFailed = reason => { noticeView.text = reason; };
    }

    public void OnButtonClicked()
    {
        _certificator.SendLoginRequest(id.text, pw.text);
    }

    public void OnSignInOk()
    {
        if (id.text.Length > 1 && pw.text.Length > 1)
        {
            _certificator.Send(new SignUp(id.text, pw.text), "Proxy");
            noticeView.text = "가입되었습니다.";
        }
        else
            noticeView.text = "ID와 PW를 최소 한글자 이상 입력해주세요.";
    }

    public void OnCustomizing()
    {
        SceneManager.LoadScene("Customizing");
    }
}