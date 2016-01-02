using System;
using TeraTaleNet;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkProgramUnity : MonoBehaviour, MessageHandler
{
    static public NetworkProgramUnity currentInstance;
    public string userName;
    protected Messenger _messenger;
    NetworkPrefabManager _prefabManager;
    bool _stopped = false;

    public bool stopped { get { return _stopped; } }
    public Dictionary<int, NetworkSignaller> signallersByID { get; set; }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnEnd();

    public void Send(Packet packet)
    {
        _messenger.Send("Proxy", packet);
    }

    void Awake()
    {
        signallersByID = new Dictionary<int, NetworkSignaller>();//must locate in Awake()
        DontDestroyOnLoad(gameObject.transform.root);
    }

    void Start()
    {
        currentInstance = this;

        _prefabManager = FindObjectOfType<NetworkPrefabManager>();
        _messenger = new Messenger(this);
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void OnDestroy()
    {
        try
        {
            OnEnd();
            StopAllCoroutines();
            _messenger.Dispose();
        }
        finally
        {
            History.Save();
        }
    }

    protected void Stop()
    {
        _stopped = true;
        Destroy(gameObject);
    }

    void MessageHandler.RPCHandler(TeraTaleNet.RPC rpc)
    {
        signallersByID[rpc.signallerID].SendMessage(rpc.GetType().Name, rpc);
    }

    public void RegisterSignaller(NetworkSignaller signaller)
    {
        signallersByID.Add(signaller._networkID, signaller);
    }
}