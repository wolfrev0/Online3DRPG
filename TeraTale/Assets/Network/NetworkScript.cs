using UnityEngine;
using System.Collections;
using TeraTaleNet;

public abstract class NetworkScript : MonoBehaviour
{
    public int networkID;
    public string owner = null;

    static protected bool? _isServer;
    bool _registered = false;
    bool _destroyed = false;

    public bool isMine { get { return userName == owner; } }
    static protected bool isServer
    {
        get
        {
            if (_isServer == null)
                _isServer = NetworkProgramUnity.currentInstance is Client == false;
            return _isServer == true;
        }
    }
    static protected string userName { get; set; }

    protected IEnumerator Start()
    {
        while (NetworkProgramUnity.currentInstance == null)
            yield return new WaitForSeconds(0);
        RegisterToProgram();
    }

    public void RegisterToProgram()
    {
        if (_registered == false)
            NetworkProgramUnity.currentInstance.RegisterSignaller(this);
        _registered = true;
    }

    protected void OnDestroy()
    {
        if (_destroyed == false)
            NetworkProgramUnity.currentInstance.UnregisterSignaller(this);
        _destroyed = true;
    }

    protected void Send(Packet packet)
    {
        NetworkProgramUnity.currentInstance.Send(packet);
    }

    protected void Send(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = networkID;
        rpc.sender = userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }

    public void NetworkInstantiate(NetworkScript prefab)
    {
        Send(new NetworkInstantiate(prefab.name));
    }

    public void NetworkInstantiate(NetworkInstantiate info)
    {
        var instance = Instantiate(Resources.Load<NetworkScript>("Prefabs/" + info.pfName));
        instance.networkID = info.networkID;
        instance.owner = info.sender;
        instance.RegisterToProgram();
    }

    protected void NetworkDestroy(NetworkScript instance)
    {
        instance.Destroy();
    }

    public void Destroy()
    {
        Send(new RemoveBufferedRPC(userName, "NetworkInstantiate", networkID));
        Send(new NetworkDestroy(networkID));
        Destroy(gameObject);
        OnDestroy();
    }

    public void NetworkDestroy(NetworkDestroy info)
    {
        Destroy(NetworkProgramUnity.currentInstance.signallersByID[info.networkID].gameObject);
    }

    protected void Sync(ref object obj)
    {
    }
}