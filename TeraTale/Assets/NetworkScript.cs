using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public abstract class NetworkScript : MonoBehaviour
{
    public int _networkID = 0;
    public string _owner = null;

    bool registered = false;

    public bool isMine { get { return NetworkProgramUnity.currentInstance.userName == _owner; } }

    protected IEnumerator Start()
    {
        while (NetworkProgramUnity.currentInstance == null)
            yield return new WaitForSeconds(0);
        RegisterToProgram();
    }

    public void RegisterToProgram()
    {
        if (registered == false)
            NetworkProgramUnity.currentInstance.RegisterSignaller(this);
        registered = true;
    }

    protected void Send(Packet packet)
    {
        NetworkProgramUnity.currentInstance.Send(packet);
    }

    protected void Send(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = _networkID;
        rpc.sender = NetworkProgramUnity.currentInstance.userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }

    protected void NetworkInstantiate(NetworkScript prefab)
    {
        int prefabIndex = -1;
        for (int i = 0; i < NetworkPrefabManager.instance.prefabs.Length; i++)
        {
            if (NetworkPrefabManager.instance.prefabs[i] == prefab)
                prefabIndex = i;
        }
        if (prefabIndex < 0)
            throw new ArgumentException("You tried instantiating not registered prefab. Please register prefab at PrefabManager.");
        Send(new NetworkInstantiate(RPCType.AllBuffered, prefabIndex));
    }

    public void NetworkInstantiate(NetworkInstantiate info)
    {
        var pf = NetworkPrefabManager.instance.prefabs[info.index];
        pf.enabled = false;
        var instance = Instantiate(pf);
        instance._networkID = info.networkID;
        instance._owner = info.sender;
        instance.RegisterToProgram();
        instance.enabled = true;
    }
}