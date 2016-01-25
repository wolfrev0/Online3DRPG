using UnityEngine;
using System.Collections;
using TeraTaleNet;
using System;
using System.Reflection;

public abstract class NetworkScript : MonoBehaviour
{
    public int networkID;
    public string owner = null;

    static protected bool? _isServer;
    bool _registered = false;
    bool _destroyed = false;

    public bool isMine { get { return userName == owner; } }
    static public bool isServer
    {
        get
        {
            if (_isServer == null)
                _isServer = NetworkProgramUnity.currentInstance is Client == false;
            return _isServer == true;
        }
    }
    static public bool isLocal { get { return !isServer; } }
    static public string userName { get; set; }

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

    public void Send(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = networkID;
        rpc.sender = userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }

    public void NetworkInstantiate(NetworkScript prefab)
    {
        var ni = new NetworkInstantiate(prefab.name, new NullPacket(), "");
        Send(ni);
    }

    public void NetworkInstantiate(NetworkScript prefab, Packet callbackArg)
    {
        var ni = new NetworkInstantiate(prefab.name, callbackArg, "");
        Send(ni);
    }

    public void NetworkInstantiate(NetworkScript prefab, string callback)
    {
        var ni = new NetworkInstantiate(prefab.name, new NullPacket(), callback);
        Send(ni);
    }

    public void NetworkInstantiate(NetworkScript prefab, Packet callbackArg, string callback)
    {
        var ni = new NetworkInstantiate(prefab.name, callbackArg, callback);
        Send(ni);
    }

    public void NetworkInstantiate(NetworkInstantiate info)
    {
        var instance = Instantiate(Resources.Load<NetworkScript>("Prefabs/" + info.pfName));
        instance.networkID = info.networkID;
        instance.owner = info.sender;
        instance.RegisterToProgram();
        if (info.callbackArg.body.GetType() != typeof(NullPacket))
            instance.SendMessage("OnNetInstantiate", info.callbackArg.body, SendMessageOptions.DontRequireReceiver);
        if (info.callback != "")
            SendMessage(info.callback, instance);
    }

    protected void NetworkDestroy(NetworkScript instance)
    {
        if (instance)
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

    protected void Sync(string member)
    {
        if (isLocal)
            Send(new Sync(Application.loadedLevelName, member));
    }

    public void Sync(Sync sync)
    {
        if(isServer)
        {
            sync.receiver = sync.sender;

            var tokens = sync.member.Split(new[] { '.' });
            object instance = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            foreach (var token in tokens)
            {
                if (field == null && property == null)
                {
                    instance = this;
                    field = GetType().GetField(token);
                    property = GetType().GetProperty(token);
                }
                else
                {
                    if (field != null)
                    {
                        instance = field.GetValue(instance);
                        field = field.FieldType.GetField(token);
                        property = field.FieldType.GetProperty(token);
                    }
                    else
                    {
                        instance = property.GetValue(instance, null);
                        field = property.PropertyType.GetField(token);
                        property = property.PropertyType.GetProperty(token);
                    }
                }
            }
            if (field != null)
            {
                sync.packet = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + field.FieldType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), field.GetValue(instance));
                Send(sync);
            }
            else
            {
                sync.packet = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + property.PropertyType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), property.GetValue(instance, null));
                Send(sync);
            }
        }
        else
        {
            var tokens = sync.member.Split(new[] { '.' });
            object instance = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            foreach (var token in tokens)
            {
                if (field == null && property == null)
                {
                    instance = this;
                    field = GetType().GetField(token);
                    property = GetType().GetProperty(token);
                }
                else
                {
                    if (field != null)
                    {
                        instance = field.GetValue(instance);
                        field = field.FieldType.GetField(token);
                        property = field.FieldType.GetProperty(token);
                    }
                    else
                    {
                        instance = property.GetValue(instance, null);
                        field = property.PropertyType.GetField(token);
                        property = property.PropertyType.GetProperty(token);
                    }
                }
            }
            if(field != null)
            {
                field.SetValue(instance, sync.packet.body.GetType().GetField("value").GetValue(sync.packet.body));
                OnSynced(sync);
            }
            else
            {
                property.SetValue(instance, sync.packet.body.GetType().GetField("value").GetValue(sync.packet.body), null);
                OnSynced(sync);
            }
        }
    }

    protected virtual void OnSynced(Sync sync) { }
}