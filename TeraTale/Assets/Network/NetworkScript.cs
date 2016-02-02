using UnityEngine;
using System.Collections;
using TeraTaleNet;
using System;
using System.Reflection;

public abstract class NetworkScript : MonoBehaviour
{
    [SerializeField]
    int _networkID;
    [SerializeField]
    string _owner = null;
    public int networkID { get { return _networkID; } set { _networkID = value; } }
    public string owner { get { return _owner; } set { _owner = value; } }

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

    protected virtual void OnNetworkDestroy()
    {
        NetworkProgramUnity.currentInstance.UnregisterSignaller(this);
        _destroyed = true;
    }

    protected void Start()
    {
        StartCoroutine(StartSub());
    }

    protected void OnDestroy()
    {
        if (_destroyed == false)
            OnNetworkDestroy();
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

    protected void Send(Packet packet, string server = "Proxy")
    {
        NetworkProgramUnity.currentInstance.Send(packet, server);
    }

    public void Send(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = networkID;
        rpc.sender = userName;
        NetworkProgramUnity.currentInstance.Send(new Packet(rpc));
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
        if (_destroyed == false)
            OnNetworkDestroy();
    }

    public void NetworkDestroy(NetworkDestroy info)
    {
        Destroy(NetworkProgramUnity.currentInstance.signallersByID[info.networkID].gameObject);
        if (_destroyed == false)
            OnNetworkDestroy();
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
                var value = field.GetValue(instance);
                var autoSerializable = value as IAutoSerializable;
                if (autoSerializable != null)
                    sync.data = autoSerializable;
                else
                    sync.data = (Body)Activator.CreateInstance(Type.GetType("TeraTaleNet.Serializable" + field.FieldType.Name + ", TeraTaleNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), field.GetValue(instance));
                Send(sync);
            }
            else
            {
                var value = property.GetValue(instance, null);
                var autoSerializable = value as IAutoSerializable;
                if (autoSerializable != null)
                    sync.data = autoSerializable;
                else
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
                var value = field.GetValue(instance);
                var autoSerializable = value as IAutoSerializable;
                if (autoSerializable != null)
                    field.SetValue(instance, sync.data);
                else
                    field.SetValue(instance, sync.data.GetType().GetField("value").GetValue(sync.data));
                OnSynced(sync);
            }
            else
            {
                var value = property.GetValue(instance, null);
                var autoSerializable = value as IAutoSerializable;
                if (autoSerializable != null)
                    property.SetValue(instance, sync.data, null);
                else
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