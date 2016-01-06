using System;
using UnityEngine;
using TeraTaleNet;

public class NetworkPrefabManager : NetworkScript
{
    static NetworkPrefabManager _instance;
    static public NetworkPrefabManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<NetworkPrefabManager>();
            return _instance;
        }
    }

    public NetworkScript[] prefabs;

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
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