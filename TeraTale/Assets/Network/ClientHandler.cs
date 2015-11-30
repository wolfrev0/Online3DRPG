using UnityEngine;
using TeraTaleNet;

public partial class Client : NetworkScript
{
    public class ClientHandler : MessageHandler
    {
        Client _body;

        public ClientHandler(Client net)
        {
            _body = net;
        }
    }
}