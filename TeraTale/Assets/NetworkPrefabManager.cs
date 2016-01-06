using System;
using UnityEngine;
using TeraTaleNet;

public class NetworkPrefabManager : NetworkScript
{
    static NetworkPrefabManager _instance;

    public NetworkScript[] prefabs;

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
    }

    static public void NetworkInstantiate(NetworkScript prefab)
    {
        if (_instance == null)
            _instance = FindObjectOfType<NetworkPrefabManager>();

        int prefabIndex = -1;
        for (int i = 0; i < _instance.prefabs.Length; i++)
        {
            if (_instance.prefabs[i] == prefab)
                prefabIndex = i;
        }
        if (prefabIndex < 0)
            throw new ArgumentException("You tried instantiating not registered prefab. Please register prefab at PrefabManager.");
        _instance.SendRPC(new NetworkInstantiate(RPCType.AllBuffered, prefabIndex));
    }

    public void NetworkInstantiate(NetworkInstantiate info)
    {
        prefabs[info.index].enabled = false;
        var instance = Instantiate(prefabs[info.index]);
        instance._networkID = info.networkID;
        instance._owner = info.sender;
        instance.RegisterToProgram();
        instance.enabled = true;
    }
}