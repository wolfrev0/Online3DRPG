using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TeraTaleNet;

public class NetworkManager : UnityServer
{
    public PacketStream stream;
    Messenger _messenger = new Messenger();

    protected override void OnStart()
    {
        _messenger.Register("", stream);
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
}