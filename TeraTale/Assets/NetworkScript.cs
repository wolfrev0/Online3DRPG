using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;
using UnityEngine.SceneManagement;

public abstract class NetworkScript : MonoBehaviour
{
    public int networkID;
    public string owner = null;

    bool registered = false;
    bool destroyed = false;

    public bool isMine { get { return userName == owner; } }
    static protected string userName { get { return NetworkProgramUnity.currentInstance.userName; } }

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

    void OnDestroy()
    {
        if (destroyed == false)
            NetworkProgramUnity.currentInstance.UnregisterSignaller(this);
    }

    static protected void Send(Packet packet)
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

    protected void NetworkDestroy(NetworkScript instance)
    {
        instance.Destroy();
    }

    public void Destroy()
    {
        destroyed = true;
        Send(new RemoveBufferedRPC(userName, "NetworkInstantiate", networkID));
        Send(new NetworkDestroy(RPCType.Others, networkID));
        Destroy(gameObject);
        NetworkProgramUnity.currentInstance.UnregisterSignaller(this);
    }

    public void NetworkDestroy(NetworkDestroy info)
    {
        Destroy(NetworkProgramUnity.currentInstance.signallersByID[info.networkID].gameObject);
    }

    static public void SwitchWorld(string world)
    {
        Player.FindPlayerByName(userName).Destroy();
        Send(new SwitchWorld(userName, world));
        SceneManager.LoadScene(world);
        NetworkPrefabManager.instance.NetworkInstantiate(NetworkPrefabManager.instance.prefabs[0]);
    }
}