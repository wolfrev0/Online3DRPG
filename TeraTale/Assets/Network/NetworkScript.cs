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

    protected void Start()
    {
        StartCoroutine(StartSub());
    }

    IEnumerator StartSub()
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

    public void NetworkInstantiate(NetworkScript prefab, IAutoSerializable callbackArg)
    {
        var ni = new NetworkInstantiate(prefab.name, callbackArg, "");
        Send(ni);
    }

    public void NetworkInstantiate(NetworkScript prefab, string callback)
    {
        var ni = new NetworkInstantiate(prefab.name, new NullPacket(), callback);
        Send(ni);
    }

    public void NetworkInstantiate(NetworkScript prefab, IAutoSerializable callbackArg, string callback)
    {
        var ni = new NetworkInstantiate(prefab.name, callbackArg, callback);
        Send(ni);
    }

    public void NetworkInstantiate(NetworkInstantiate info)
    {
        var pf = Resources.Load<NetworkScript>("Prefabs/" + info.pfName);
        pf.enabled = false;
        var instance = Instantiate(pf);
        instance.networkID = info.networkID;
        instance.owner = info.sender;
        instance.RegisterToProgram();
        instance.enabled = true;
        if (info.callbackArg.GetType() != typeof(NullPacket))
            instance.SendMessage("OnNetInstantiate", info.callbackArg, SendMessageOptions.DontRequireReceiver);
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
            Send(new Sync(RPCType.Specific, Application.loadedLevelName, member));
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
                sync.data = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + field.FieldType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), field.GetValue(instance));
                Send(sync);
            }
            else
            {
                sync.data = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + property.PropertyType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), property.GetValue(instance, null));
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
                field.SetValue(instance, sync.data.GetType().GetField("value").GetValue(sync.data));
                OnSynced(sync);
            }
            else
            {
                property.SetValue(instance, sync.data.GetType().GetField("value").GetValue(sync.data), null);
                OnSynced(sync);
            }
        }
    }

    public void SetActive(SetActive rpc)
    {
        gameObject.SetActive(rpc.value);
    }

    protected virtual void OnSynced(Sync sync) { }
}