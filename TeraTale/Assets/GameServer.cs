using UnityEngine;
using System;
using System.Collections.Generic;
using TeraTaleNet;

public class GameServer : UnityServer
{
    Messenger _messenger = new Messenger();
    Dictionary<string, HashSet<string>> playersByWorld;

    protected override void OnStart()
    {
        _messenger.Register("Database", Connect("127.0.0.1", Port.DatabaseForGameServer));
        Debug.Log("Database connected.");
        _messenger.Register("Login", Connect("127.0.0.1", Port.LoginForGameServer));
        Debug.Log("Login connected.");
        _messenger.Register("Proxy", Listen("127.0.0.1", Port.GameServer, 1));
        Debug.Log("Proxy connected.");

        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.PlayerInfoResponse, OnPlayerInfoResponse);
        StartCoroutine(_messenger.DispatcherCoroutine("Database", delegates));

        delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.PlayerLogin, OnPlayerLogin);
        StartCoroutine(_messenger.DispatcherCoroutine("Login", delegates));

        delegates = new Dictionary<PacketType, PacketDelegate>();
        StartCoroutine(_messenger.DispatcherCoroutine("Proxy", delegates));

        var login = FindObjectOfType<Certificator>();
        login.enabled = true;

        _messenger.Start();
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Join();
    }

    protected override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    void OnPlayerLogin(Packet packet)
    {
        PlayerLogin login = (PlayerLogin)packet.body;
        _messenger.Send("Database", new Packet(new PlayerInfoRequest(login.nickName)));
        //Sync Data Get
    }

    void OnPlayerInfoResponse(Packet packet)
    {
        PlayerInfoResponse info = (PlayerInfoResponse)packet.body;
    }
}
