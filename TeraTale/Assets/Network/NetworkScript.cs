using UnityEngine;
using System.Collections;
using TeraTaleNet;
using System;

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
    static protected bool isLocal { get { return !isServer; } }
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

    protected void Sync(string fieldName)
    {
        Send(new Sync(Application.loadedLevelName, fieldName));
    }

    public void Sync(Sync sync)
    {
        if(isServer)
        {
            sync.receiver = sync.sender;
            var field = GetType().GetField(sync.field);
            sync.packet = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + field.FieldType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), field.GetValue(this));
            Send(sync);
        }
        else
        {
            var field = GetType().GetField(sync.field);
            field.SetValue(this, sync.packet.body.GetType().GetField("value").GetValue(sync.packet.body));
        }
    }
}