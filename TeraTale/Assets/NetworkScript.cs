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

    protected void SendRPC(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = _networkID;
        rpc.sender = NetworkProgramUnity.currentInstance.userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }

    static public void NetworkInstantiate(NetworkScript prefab)
    {
        int prefabIndex = -1;
        for (int i = 0; i < NetworkPrefabManager.instance.prefabs.Length; i++)
        {
            if (NetworkPrefabManager.instance.prefabs[i] == prefab)
                prefabIndex = i;
        }
        if (prefabIndex < 0)
            throw new ArgumentException("You tried instantiating not registered prefab. Please register prefab at PrefabManager.");
        NetworkPrefabManager.instance.SendRPC(new NetworkInstantiate(RPCType.AllBuffered, prefabIndex));
    }
}