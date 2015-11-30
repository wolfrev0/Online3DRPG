using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;

public partial class Forest : NetworkScript
{
    ForestHandler _handler;
    NetworkAgent _agent = new NetworkAgent();
    Messenger _messenger;
    HashSet<string> players;

    protected override void OnStart()
    {
        _handler = new ForestHandler(this);
        _messenger = new Messenger(_handler);

        PacketStream stream;
        ConnectorInfo info;

        stream = _agent.Connect("127.0.0.1", Port.Database);
        stream.Write(new ConnectorInfo("Forest"));
        _messenger.Register("Database", stream);
        Console.WriteLine("Database connected.");

        _agent.Bind("127.0.0.1", Port.Forest, 1);
        stream = _agent.Listen();
        info = (ConnectorInfo)stream.Read().body;
        _messenger.Register(info.name, stream);
        Console.WriteLine(info.name + " connected.");

        foreach(var key in _messenger.Keys)
            StartCoroutine(Dispatcher(key));

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
