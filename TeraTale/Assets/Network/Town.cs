using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;

public class Town : UnityServer
{
    TownHandler _handler;
    Messenger _messenger;
    Dictionary<string, HashSet<string>> playersByWorld;
    bool _disposed = false;

    protected override void OnStart()
    {
        _handler = new TownHandler();
        _messenger = new Messenger(_handler);

        _messenger.Register("Database", Connect("127.0.0.1", Port.DatabaseForTown));
        Debug.Log("Database connected.");

        Bind("127.0.0.1", Port.TownForProxy, 1);
        _messenger.Register("Proxy", Listen());
        Debug.Log("Proxy connected.");

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
