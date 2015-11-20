using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class Certificator : UnityServer
{
    Messenger _messenger = new Messenger();

    protected override void OnStart()
    {
        _messenger.Register("Proxy", ConnectToProxy());


        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.LoginResponse, OnLoginResponse);
        StartCoroutine(Dispatcher("Proxy", delegates));

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

    PacketStream ConnectToProxy()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.Proxy);
        Debug.Log("Proxy Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    void OnLoginResponse(Packet packet)
    {
        LoginResponse response = (LoginResponse)packet.body;
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
        _messenger.Send("Proxy", new Packet(new LoginRequest(id, pw)));
    }
}