using System;
using UnityEngine;
using TeraTaleNet;

public class NetworkPrefabManager : MonoBehaviour
{
    static NetworkPrefabManager _instance;

    public NetworkSignaller[] prefabs;
    NetworkSignaller _signaller;

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
        _signaller = GetComponent<NetworkSignaller>();
    }

    static public void NetworkInstantiate(NetworkSignaller prefab)
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
        _instance._signaller.SendRPC(new NetworkInstantiate(RPCType.AllBuffered, prefabIndex));
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