using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class Certificator : UnityServer, MessageListener
{
    Messenger _messenger;
    int _confirmID;
    bool _disposed = false;
    object _locker = new object();

    protected override void OnStart()
    {
        _messenger = new Messenger(this);
        _messenger.Register("Proxy", Connect("127.0.0.1", Port.Proxy));
        Debug.Log("Proxy connected.");
        
        StartCoroutine(Dispatcher("Proxy"));

        _messenger.Start();
    }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            lock (_locker)
                _messenger.DispatcherCoroutine(key);
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

    [TeraTaleNet.RPC]
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

    [TeraTaleNet.RPC]
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