using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.Text;

public class Request : MonoBehaviour
{
    Dictionary<string, string> dic;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUILayout.Button("Login"))
        {
            SendLogin();
        }
    }

    public void SendLogin()
    {
        dic = new Dictionary<string, string>();
        dic.Add("id", "root");
        dic.Add("pw", "1234");

        StartCoroutine(SendLoginData(JsonFx.Json.JsonWriter.Serialize(dic)));
    }

    private IEnumerator SendLoginData(string data)
    {
        string url = "http://127.0.0.1/book.php";

        Encoding encoding = new System.Text.UTF8Encoding();
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("ContentsType", "test/json");
        header.Add("ContentsLength", data.Length.ToString());

        WWW www = new WWW(url, encoding.GetBytes(data), dic);

        StartCoroutine(Progress(www));

        yield return www;

        if (www.isDone && www.error == null && www != null)
        {
            Respown.ReceiveLogin(www.text);
        }
        else
        {
            Debug.Log(www.error);
        }

    }

    public IEnumerator Progress(WWW www)
    {
        while (!www.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}