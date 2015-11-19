using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class GameServer : MonoBehaviour, IServer
{
    Messenger _messenger = new Messenger();
    bool stopped = false;

    Dictionary<string, HashSet<string>> playersByWorld;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Register("Database", ConnectToDatabase());
        Register("Login", ConnectToLogin());
        Register("Proxy", ListenProxy());

        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.PlayerInfoResponse, OnPlayerInfoResponse);
        StartCoroutine(Dispatcher("Database", delegates));

        delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.PlayerLogin, OnPlayerLogin);
        StartCoroutine(Dispatcher("Login", delegates));

        delegates = new Dictionary<PacketType, PacketDelegate>();
        StartCoroutine(Dispatcher("Proxy", delegates));

        var login = FindObjectOfType<Certificator>();
        login.enabled = true;

        _messenger.Start();
    }

    //void OnApplicationQuit()
    //{
    //    _messenger.Join();
    //}

    void OnDestroy()
    {
        _messenger.Join();
    }

    void Register(string key, PacketStream stream)
    {
        _messenger.Register(key, stream);
    }

    void Send(string key, Packet packet)
    {
        _messenger.Send(key, packet);
    }

    void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    IEnumerator Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegateByPacketType)
    {
        while (stopped == false)
        {
            while (_messenger.CanReceive(key))
            {
                var packet = _messenger.Receive(key);
                delegateByPacketType[packet.header.type](packet);
            }
            yield return new WaitForSeconds(0);
        }
    }

    void Stop()
    {
        stopped = true;
        Application.Quit();
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
        OnUpdate();
    }

    void OnPlayerLogin(Packet packet)
    {
        PlayerLogin login = (PlayerLogin)packet.body;
        Send("Database", new Packet(new PlayerInfoRequest(login.nickName)));
        //Sync Data Get
    }

    void OnPlayerInfoResponse(Packet packet)
    {
        PlayerInfoResponse info = (PlayerInfoResponse)packet.body;
    }
}
