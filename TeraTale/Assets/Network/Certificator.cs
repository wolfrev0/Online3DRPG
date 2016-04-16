using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TeraTaleNet;

public class Certificator : NetworkProgramUnity
{
    public InputField proxyIP;
    NetworkAgent _agent = new NetworkAgent();
    object _locker = new object();
    int _confirmID;

    protected override void OnStart()
    {
        _messenger.Register("Proxy", _agent.Connect(proxyIP.text, Port.Proxy));
        Console.WriteLine("Proxy connected.");

        foreach (var key in _messenger.Keys)
            StartCoroutine(Dispatcher(key));

        _messenger.Start();
    }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            lock (_locker)
            {
                while (_messenger.CanReceive(key))
                {
                    _messenger.DispatcherCoroutine(key);
                    while (Application.isLoadingLevel)
                        yield return null;
                }
            }
            yield return null;
        }
    }

    protected override void OnEnd()
    { }

    protected override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new LoginQuery(id, pw, _confirmID));
    }

    public void Dispose()
    {
        _messenger.Join();
        _agent.Dispose();
        GC.SuppressFinalize(this);
    }

    void ConfirmID(Messenger messenger, string key, ConfirmID confirmID)
    {
        _confirmID = confirmID.id;
    }

    void LoginAnswer(Messenger messenger, string key, LoginAnswer answer)
    {
        //If failed, Show Failed Message.
        if (answer.accepted)
        {
            SceneManager.LoadScene(answer.world);

            userName = answer.name;

            var net = FindObjectOfType<Client>();
            lock (_locker)
                net.stream = messenger.Unregister("Proxy");
            userName = answer.name;
            net.signallersByID = signallersByID;
            net.enabled = true;            
        }
    }
}