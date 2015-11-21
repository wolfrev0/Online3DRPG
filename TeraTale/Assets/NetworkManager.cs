using System;
using TeraTaleNet;

public class NetworkManager : UnityServer, MessageListener
{
    public PacketStream stream;
    Messenger _messenger;
    bool _disposed = false;

    protected override void OnStart()
    {
        _messenger = new Messenger(this);

        _messenger.Register("", stream);
        _messenger.Start();
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Dispose();
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