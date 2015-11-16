using UnityEngine;
using System;
using System.Net.Sockets;
using LoboNet;
using TeraTaleNet;

public class LoginManager : MonoBehaviour
{
    static LoginManager _instance;
    Messenger _proxy;

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
            _proxy = ConnectToProxy();
            _proxy.Start();
        }
        catch(SocketException e)
        {
            Debug.LogException(e);
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        _proxy.Join();
    }

    Messenger ConnectToProxy()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)TargetPort.Client);
        Debug.Log("Proxy Connected.");
        _connecter.Dispose();

        return new Messenger(new PacketStream(connection));
    }

    void Update ()
    {
        if (_proxy.CanReceive())
        {
            var packet = _proxy.Receive();
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
            Debug.Log("Rejected.");
        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        _proxy.Send(new Packet(new LoginRequest(id, pw)));
    }
}