using UnityEngine;
using System.IO;
using System.Diagnostics;

public class ServerClientSelectionHandler : MonoBehaviour
{
    public void OnServerClicked()
    {
        var database = new Process();
        database.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Database\\Database\\bin\\Debug\\Database.exe";
        database.Start();

        var login = new Process();
        login.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Login\\Login\\bin\\Debug\\Login.exe";
        login.Start();

        var proxy = new Process();
        proxy.StartInfo.FileName = "C:\\Users\\Lobo\\Desktop\\Projects\\TeraTale\\Proxy\\Proxy\\bin\\Debug\\Proxy.exe";
        proxy.Start();
    }

    public void OnClientClicked()
    {
        Application.LoadLevel("Login");
    }
}
