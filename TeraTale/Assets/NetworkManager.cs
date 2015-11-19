using UnityEngine;
using System;
using LoboNet;
using TeraTaleNet;

public class NetworkManager : MonoBehaviour
{
    public PacketStream stream;
    Messenger _messenger = new Messenger();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _messenger.Register("", stream);
        _messenger.Start();
    }

    void OnApplicationQuit()
    {
        _messenger.Join();
    }

    void Update()
    {
        if (_messenger.CanReceive(""))
        {
            var packet = _messenger.Receive("");
            switch (packet.header.type)
            {
                default:
                    throw new ArgumentException("Received invalid packet type.");
            }
        }
    }
}