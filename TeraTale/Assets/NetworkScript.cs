using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public abstract class NetworkScript : MonoBehaviour
{
    public int networkID;
    public string owner = null;

    bool registered = false;

    protected string userName { get { return NetworkProgramUnity.currentInstance.userName; } }
    protected bool isMine { get { return userName == owner; } }

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
        rpc.signallerID = networkID;
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
        instance.networkID = info.networkID;
        instance.owner = info.sender;
        instance.RegisterToProgram();
        instance.enabled = true;
    }

    public void NetworkDestroy()
    {
        Send(new RemoveBufferedRPC(userName, "NetworkInstantiate", networkID));
        Send(new NetworkDestroy(RPCType.All, networkID));
    }

    public void NetworkDestroy(NetworkDestroy info)
    {
        Destroy(NetworkProgramUnity.currentInstance.signallersByID[info.networkID].gameObject);
    }
}