using UnityEngine;
using TeraTaleNet;

public class ClientHandler : MessageHandler
{
    Client _net;

    public ClientHandler(Client net)
    {
        _net = net;
    }
}