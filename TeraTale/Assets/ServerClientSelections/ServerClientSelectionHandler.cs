using UnityEngine;
using System.Diagnostics;

public class ServerClientSelectionHandler : MonoBehaviour
{
    public Town town;
    public Forest forest;

    public void OnDatabase()
    {
        var database = new Process();
        database.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Database\\Database\\bin\\Debug\\Database.exe";
        database.Start();
    }

    public void OnLogin()
    {
        var login = new Process();
        login.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Login\\Login\\bin\\Debug\\Login.exe";
        login.Start();
    }

    public void OnTown()
    {
        town.enabled = true;
        Application.LoadLevel("Town");
    }

    public void OnForest()
    {
        forest.enabled = true;
        Application.LoadLevel("Forest");
    }

    public void OnProxy()
    {
        var proxy = new Process();
        proxy.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Proxy\\Proxy\\bin\\Debug\\Proxy.exe";
        proxy.Start();
    }

    public void OnClient()
    {
        FindObjectOfType<Certificator>().enabled = true;
        Application.LoadLevel("Login");
    }
}
