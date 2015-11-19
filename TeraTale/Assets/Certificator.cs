using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class Certificator : MonoBehaviour, IServer
{
    Messenger<string> _messenger = new Messenger<string>();
    bool stopped = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Register("Proxy", ConnectToProxy());


        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.LoginResponse, OnLoginResponse);
        StartCoroutine(Dispatcher("Proxy", delegates));

        _messenger.Start();
    }

    //void OnApplicationQuit()
    //{
    //    _messenger.Join();
    //}

    void OnDestroy()
    {
        _messenger.Join();
    }

    void Register(string key, PacketStream stream)
    {
        _messenger.Register(key, stream);
    }

    void Send(string key, Packet packet)
    {
        _messenger.Send(key, packet);
    }

    void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    IEnumerator Dispatcher(string key, Dictionary<PacketType, PacketDelegate> delegateByPacketType)
    {
        while (stopped == false)
        {
            while (_messenger.CanReceive(key))
            {
                var packet = _messenger.Receive(key);
                delegateByPacketType[packet.header.type](packet);
            }
            yield return new WaitForSeconds(0);
        }
    }

    void Stop()
    {
        stopped = true;
        Application.Quit();
    }

    PacketStream ConnectToProxy()
    {
        var _connecter = new TcpConnecter();
        var connection = _connecter.Connect("127.0.0.1", (ushort)Port.Proxy);
        Debug.Log("Proxy Connected.");
        _connecter.Dispose();

        return new PacketStream(connection);
    }

    void Update()
    {
        OnUpdate();
    }

    void OnLoginResponse(Packet packet)
    {
        LoginResponse response = (LoginResponse)packet.body;
        if (response.accepted)
        {
            var net = FindObjectOfType<NetworkManager>();
            net.stream = _messenger.Unregister("Proxy");
            net.enabled = true;
            Application.LoadLevel("Town");
        }
        else
        {

        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        Send("Proxy", new Packet(new LoginRequest(id, pw)));
    }
}