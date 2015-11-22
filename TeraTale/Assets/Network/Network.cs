using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class Network : UnityServer
{
    public Player pfPlayer;
    public PacketStream stream;
    NetworkHandler _handler;
    Messenger _messenger;
    bool _disposed = false;

    protected override void OnStart()
    {
        _handler = new NetworkHandler(this);
        _messenger = new Messenger(_handler);

        _messenger.Register("Proxy", stream);

        StartCoroutine(Dispatcher("Proxy"));

        _messenger.Start();
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Dispose();
    }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            _messenger.DispatcherCoroutine(key);
            yield return new WaitForSeconds(0);
        }
    }

    protected override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
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