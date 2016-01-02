using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class Client : NetworkProgramUnity
{
    public PacketStream stream;
    public NetworkSignaller pfPlayer;
    NetworkAgent _agent = new NetworkAgent();
    Messenger _messenger;

    protected override void OnStart()
    {
        _messenger = new Messenger(this);

        _messenger.Register("Proxy", stream);

        StartCoroutine(Dispatcher("Proxy"));

        _messenger.Start();
        
        NetworkPrefabManager.NetworkInstantiate(pfPlayer);
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Dispose();
    }

    public override void Send(Packet packet)
    {
        _messenger.Send("Proxy", packet);
    }

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