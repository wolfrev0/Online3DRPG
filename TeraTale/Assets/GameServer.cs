using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class GameServer : UnityServer
{
    Messenger _messenger = new Messenger();
    Dictionary<string, HashSet<string>> playersByWorld;

    protected override void OnStart()
    {
        _messenger.Register("Database", ConnectToDatabase());
        _messenger.Register("Login", ConnectToLogin());
        _messenger.Register("Proxy", ListenProxy());

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

    IEnumerator Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegateByPacketType)
    {
        while (true)
        {
            while (_messenger.CanReceive(key))
            {
                var packet = _messenger.Receive(key);
                delegateByPacketType[packet.header.type](packet);
            }
            yield return new WaitForSeconds(0);
        }
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
