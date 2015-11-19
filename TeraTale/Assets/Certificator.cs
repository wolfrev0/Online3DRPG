using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LoboNet;
using TeraTaleNet;

public class Certificator : MonoBehaviour, IServer
{
    Messenger _messenger = new Messenger();
    string _confirmID = "INVALID";
    bool stopped = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _messenger.Register("Proxy", ConnectToProxy());

        var delegates = new Dictionary<PacketType, PacketDelegate>();
        delegates.Add(PacketType.LoginResponse, OnLoginResponse);
        delegates.Add(PacketType.ConfirmID, OnConfirmID);
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

    void OnConfirmID(Packet packet)
    {
        ConfirmID id = (ConfirmID)packet.body;
        _confirmID = id.confirmID;
        Debug.Log(_confirmID);
    }

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new Packet(new LoginRequest(id, pw, _confirmID)));
    }
}