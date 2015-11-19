using UnityEngine;
using System;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class GameServer : MonoBehaviour
{
    Messenger<string> _messenger = new Messenger<string>();
    PlayerLogin lastestLogin = null;
    Dictionary<string, HashSet<string>> playersByWorld;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _messenger.Register("Database", ConnectToDatabase());
        _messenger.Register("Login", ConnectToLogin());
        _messenger.Register("Proxy", ListenProxy());
        var login = FindObjectOfType<LoginManager>();
        login.enabled = true;
    }

    PacketStream ConnectToDatabase()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.DatabaseForGameServer);
        Debug.Log("Database Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    PacketStream ConnectToLogin()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.LoginForGameServer);
        Debug.Log("Login Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    PacketStream ListenProxy()
    {
        var _listener = new TcpListener("127.0.0.1", (ushort)Port.GameServer, 1);
        var connection = _listener.Accept();
        Console.WriteLine("Proxy Connected.");
        _listener.Dispose();

        return new PacketStream(connection);
    }

    void Update()
    {
        while (_messenger.CanReceive("Database"))
        {
            var packet = _messenger.Receive("Database");
            switch (packet.header.type)
            {
                default:
                    throw new ArgumentException("Received invalid packet type.");
            }
        }
        while (_messenger.CanReceive("Login"))
        {
            var packet = _messenger.Receive("Login");
            switch (packet.header.type)
            {
                case PacketType.PlayerLogin:
                    OnPlayerLogin((PlayerLogin)packet.body);
                    break;
                default:
                    throw new ArgumentException("Received invalid packet type.");
            }
        }
        while (_messenger.CanReceive("Proxy"))
        {
            var packet = _messenger.Receive("Proxy");
            switch (packet.header.type)
            {
                default:
                    throw new ArgumentException("Received invalid packet type.");
            }
        }
    }

    void OnPlayerLogin(PlayerLogin login)
    {
        lastestLogin = login;
        _messenger.Send("Database", new Packet(new PlayerInfoRequest(login.nickName)));
    }

    void OnPlayerInfoResponse(PlayerInfoResponse info)
    {
        
    }
}
