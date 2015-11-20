using UnityEngine;
using System;
using System.Collections.Generic;
using TeraTaleNet;

public class Certificator : UnityServer
{
    Messenger _messenger = new Messenger();
    int _confirmID;

    protected override void OnStart()
    {
        _messenger.Register("Proxy", Connect("127.0.0.1", Port.Proxy));
        Debug.Log("Proxy connected.");

        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.LoginResponse, OnLoginResponse);
        delegates.Add(PacketType.ConfirmID, OnConfirmID);
        StartCoroutine(_messenger.DispatcherCoroutine("Proxy", delegates));

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

    void OnConfirmID(Packet packet)
    {
        _confirmID = ((ConfirmID)packet.body).id;
    }

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new Packet(new LoginRequest(id, pw, _confirmID)));
    }
}