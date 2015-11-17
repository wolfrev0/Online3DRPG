using UnityEngine;
using System;
using LoboNet;
using TeraTaleNet;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager _instance;
    public PacketStream stream;
    Messenger<string> _messenger = new Messenger<string>();

    public static NetworkManager instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<NetworkManager>();
            }
            return _instance;
        }
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