using UnityEngine;
using System;
using LoboNet;
using TeraTaleNet;

public class LoginManager : MonoBehaviour
{
    Messenger<string> _messenger = new Messenger<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _messenger.Register("Proxy", ConnectToProxy());
        _messenger.Start();
    }

    void OnDestroy()
    {
        _messenger.Join();
    }

    PacketStream ConnectToProxy()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.Proxy);
        Debug.Log("Proxy Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    void Update()
    {
        if (_messenger.CanReceive("Proxy"))
        {
            var packet = _messenger.Receive("Proxy");
            switch (packet.header.type)
            {
                case PacketType.LoginResponse:
                    OnLoginResponse((LoginResponse)packet.body);
                    break;
                default:
                    throw new ArgumentException("Received invalid packet type.");
            }
        }
    }

    void OnLoginResponse(LoginResponse response)
    {
        if (response.accepted)
        {
            var net = FindObjectOfType<NetworkManager>();
            net.stream = _messenger.Unregister("Proxy");
            net.enabled = true;
            Application.LoadLevel("Town");
        }
        else
        {

        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        var p = new Packet(new LoginRequest(id, pw));
        _messenger.Send("Proxy", p);
        Debug.Log(p.header.type);
    }
}