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
}