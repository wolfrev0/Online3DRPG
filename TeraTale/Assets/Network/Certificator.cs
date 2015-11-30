using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public partial class Certificator : NetworkScript, IDisposable
{
    CertificatorHandler _handler;
    NetworkAgent _agent = new NetworkAgent();
    Messenger _messenger;
    object _locker = new object();
    int _confirmID;

    protected override void OnStart()
    {
        _handler = new CertificatorHandler(this);
        _messenger = new Messenger(_handler);
        
        _messenger.Register("Proxy", _agent.Connect("127.0.0.1", Port.Proxy));
        Console.WriteLine("Proxy connected.");

        foreach (var key in _messenger.Keys)
            StartCoroutine(Dispatcher(key));

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

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new LoginQuery(id, pw, _confirmID));
    }

    public void Dispose()
    {
        _messenger.Join();
        _agent.Dispose();
        GC.SuppressFinalize(this);
    }
}