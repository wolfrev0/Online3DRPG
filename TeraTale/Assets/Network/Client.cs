using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public partial class Client : NetworkScript
{
    public Player pfPlayer;
    public PacketStream stream;
    ClientHandler _handler;
    NetworkAgent _agent = new NetworkAgent();
    Messenger _messenger;

    protected override void OnStart()
    {
        _handler = new ClientHandler(this);
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

    public void Dispose()
    {
        _messenger.Join();
        _agent.Dispose();
        GC.SuppressFinalize(this);
    }
}