using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;

public class Certificator : UnityServer
{
    Messenger _messenger = new Messenger();
    int _confirmID;
    bool _disposed = false;
    object _locker = new object();

    protected override void OnStart()
    {
        lock (_locker)
            _messenger.Register("Proxy", Connect("127.0.0.1", Port.Proxy));
        Debug.Log("Proxy connected.");

        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.LoginResponse, OnLoginResponse);
        delegates.Add(PacketType.ConfirmID, OnConfirmID);
        StartCoroutine(Dispatcher("Proxy", delegates));

        _messenger.Start();
    }

    IEnumerator Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegates)
    {
        while (true)
        {
            lock (_locker)
                _messenger.DispatcherCoroutine(key, delegates);
            yield return new WaitForSeconds(0);
        }
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
            lock (_locker)
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

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            try
            {
                if (disposing)
                {
                    _messenger.Join();
                }
                _disposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}