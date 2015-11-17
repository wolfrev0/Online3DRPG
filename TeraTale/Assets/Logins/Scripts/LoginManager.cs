using UnityEngine;
using System;
using System.Net.Sockets;
using System.IO;
using LoboNet;
using TeraTaleNet;

public class LoginManager : MonoBehaviour
{
    static LoginManager _instance;
    Messenger<string> _messenger = new Messenger<string>();

    public static LoginManager instance
    {
        get
        {
            if(!_instance)
            {
                _instance = FindObjectOfType<LoginManager>();
            }
            return _instance;
        }
    }

    void Start ()
    {
        try
        {
            _messenger.Register("Proxy", ConnectToProxy());
            _messenger.Start();
        }
        catch(SocketException e)
        {
            Debug.LogException(e);
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        _messenger.Join();
    }

    PacketStream ConnectToProxy()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Client);
        Debug.Log("Proxy Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    void Update ()
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
            Debug.Log("Accepted " + response.nickName);
        }
        else
        {
            Debug.Log("Rejected because " + response.reason);
        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new Packet(new LoginRequest(id, pw)));
    }
}