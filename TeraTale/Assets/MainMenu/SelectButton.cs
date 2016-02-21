using UnityEngine;
using System.Collections;

public class SelectButton : MonoBehaviour
{
    public void JoinCustomize()
    {
        Application.LoadLevel("Customizing");
    }

    public void JoinLogin()
    {
        Application.LoadLevel("Login");
    }
}
