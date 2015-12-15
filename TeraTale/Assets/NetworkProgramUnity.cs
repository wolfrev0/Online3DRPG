using System;
using TeraTaleNet;
using UnityEngine;

public abstract class NetworkProgramUnity : MonoBehaviour
{
    public string userName;
    NetworkPrefabManager _prefabManager;
    bool _stopped = false;

    public bool stopped { get { return _stopped; } }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnEnd();

    protected abstract void Send(Packet packet);

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
    }

    void Start()
    {
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
        //Player player = Instantiate(pfPlayer);
        //DontDestroyOnLoad(player.gameObject);
    }
}