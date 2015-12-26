using System;
using TeraTaleNet;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkProgramUnity : MonoBehaviour, MessageHandler
{
    public string userName;
    NetworkPrefabManager _prefabManager;
    NetworkSignaller _signaller;
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
        _prefabManager = FindObjectOfType<NetworkPrefabManager>();

        OnStart();

        _signaller = GetComponent<NetworkSignaller>();
        _signaller.Initialize(0, userName);
        _signallersByID.Add(0, _signaller);
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
        var instance = Instantiate(_prefabManager.prefabs[info.prefabIndex]);
        instance.Initialize(info.signallerID, info.owner);
        _signallersByID.Add(info.signallerID, instance);
    }
}