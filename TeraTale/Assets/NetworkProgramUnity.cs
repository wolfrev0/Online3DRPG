using System;
using TeraTaleNet;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkProgramUnity : MonoBehaviour, MessageHandler
{
    static public NetworkProgramUnity currentInstance;
    public string userName;
    NetworkPrefabManager _prefabManager;
    Dictionary<int, NetworkSignaller> _signallersByID = new Dictionary<int, NetworkSignaller>();
    bool _stopped = false;

    public bool stopped { get { return _stopped; } }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnEnd();

    public abstract void Send(Packet packet);

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
    }

    void Start()
    {
        currentInstance = this;

        _prefabManager = FindObjectOfType<NetworkPrefabManager>();
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
        _signallersByID[rpc.signallerID].SendMessage(rpc.GetType().Name, rpc);
    }

    public void NetworkInstantiate(NetworkSignaller prefab)
    {
        int prefabIndex = -1;
        for (int i = 0; i < _prefabManager.prefabs.Length; i++)
        {
            if (_prefabManager.prefabs[i] == prefab)
                prefabIndex = i;
        }
        if (prefabIndex < 0)
            throw new ArgumentException("You tried instantiating not registered prefab. Please register prefab at PrefabManager.");
        Send(new NetworkInstantiateRequest(userName, prefabIndex));
    }

    public void NetworkInstantiateInfo(Messenger messenger, string key, NetworkInstantiateInfo info)
    {
        _prefabManager.prefabs[info.prefabIndex].enabled = false;
        var instance = Instantiate(_prefabManager.prefabs[info.prefabIndex]);
        instance._networkID = info.signallerID;
        instance._owner = info.owner;
        instance.enabled = true;
    }

    public void RegisterSignaller(NetworkSignaller signaller)
    {
        _signallersByID.Add(signaller._networkID, signaller);
    }
}