using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;

public abstract class GameServer : NetworkProgramUnity
{
    public new static GameServer currentInstance
    {
        get
        {
            if (isServer)
                return (GameServer)NetworkProgramUnity.currentInstance;
            return null;
        }
    }

    NetworkAgent _agent = new NetworkAgent();

    protected override void OnStart()
    {
        userName = GetType().Name;

        PacketStream stream;
        ConnectorInfo info;

        stream = _agent.Connect("127.0.0.1", Port.Database);
        stream.Write(new ConnectorInfo(GetType().Name));
        _messenger.Register("Database", stream);
        Console.WriteLine("Database connected.");

        _agent.Bind("127.0.0.1", (Port)Enum.Parse(typeof(Port), GetType().Name), 1);
        stream = _agent.Listen();
        info = (ConnectorInfo)stream.Read().body;
        _messenger.Register(info.name, stream);
        Console.WriteLine(info.name + " connected.");

        foreach (var key in _messenger.Keys)
            StartCoroutine(Dispatcher(key));

        _messenger.Start();
    }

    protected override void OnEnd()
    {
        Item.Save();
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

    public void QuerySerializedPlayer(string name)
    {
        Send(new SerializedPlayerQuery(userName, name), "Database");
    }

    public void SavePlayer(Player player)
    {
        Send(new SerializedPlayerSave(player.name, player.Serialize()), "Database");
    }

    public void SerializedPlayerAnswer(Messenger messenger, string key, SerializedPlayerAnswer answer)
    {
        var sp = new SerializedPlayer();
        sp.data = answer.bytes;
        Player.FindPlayerByName(answer.player).SerializedPlayer(sp);
    }
}