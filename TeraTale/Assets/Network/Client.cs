using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class Client : NetworkProgramUnity
{
    public PacketStream stream;
    public NetworkScript pfPlayer;
    NetworkAgent _agent = new NetworkAgent();

    protected override void OnStart()
    {
        _messenger.Register("Proxy", stream);

        StartCoroutine(Dispatcher("Proxy"));

        _messenger.Start();
        
        NetworkPrefabManager.NetworkInstantiate(pfPlayer);
    }

    protected override void OnEnd()
    { }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            while (_messenger.CanReceive(key))
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