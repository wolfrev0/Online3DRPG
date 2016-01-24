using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class ServerClientSelectionHandler : MonoBehaviour
{
    public Town town;
    public Forest forest;

    public void OnDatabase()
    {
        var database = new Process();
        database.StartInfo.FileName = "D:\\Desktop\\Projects\\TeraTale\\Database\\Database\\bin\\Debug\\Database.exe";
        database.Start();
    }

    public void OnLogin()
    {
        var login = new Process();
        login.StartInfo.FileName = "D:\\Desktop\\Projects\\TeraTale\\Login\\Login\\bin\\Debug\\Login.exe";
        login.Start();
    }

    public void OnTown()
    {
        town.enabled = true;
        SceneManager.LoadScene("Town");
    }

    public void OnForest()
    {
        forest.enabled = true;
        SceneManager.LoadScene("Forest");
    }

    public void OnProxy()
    {
        var proxy = new Process();
        proxy.StartInfo.FileName = "D:\\Desktop\\Projects\\TeraTale\\Proxy\\Proxy\\bin\\Debug\\Proxy.exe";
        proxy.Start();
    }

    public void OnClient()
    {
        FindObjectOfType<Certificator>().enabled = true;
        SceneManager.LoadScene("Login");
    }
}
