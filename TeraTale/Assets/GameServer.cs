using UnityEngine;
using LoboNet;
using TeraTaleNet;

public class GameServer : MonoBehaviour
{
    Messenger<string> _messenger = new Messenger<string>();

    void Start()
    {
        _messenger.Register("Login", ConnectToLogin());
    }

    PacketStream ConnectToLogin()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.LoginForGameServer);
        Debug.Log("Login Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    void Update()
    {
        //if (_messenger.CanReceive("Login"))
        //{
        //    var packet = _messenger.Receive("Login");
        //    switch (packet.header.type)
        //    {
        //        case PacketType.LoginResponse:
        //            OnLoginResponse((LoginResponse)packet.body);
        //            break;
        //        default:
        //            throw new ArgumentException("Received invalid packet type.");
        //    }
        //}
    }
}
