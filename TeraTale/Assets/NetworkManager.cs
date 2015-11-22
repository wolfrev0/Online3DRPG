using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class NetworkManager : UnityServer, MessageListener
{
    public Player pfPlayer;

    public PacketStream stream;
    Messenger _messenger;
    bool _disposed = false;

    protected override void OnStart()
    {
        _messenger = new Messenger(this);

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

    [TeraTaleNet.RPC]
    void OnPlayerJoin(Messenger messenger, string key, Packet packet)
    {
        PlayerJoin join = (PlayerJoin)packet.body;
        Player player = Instantiate(pfPlayer);
        player.gameObject.name = join.nickName;
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