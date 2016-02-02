using System;
using TeraTaleNet;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkProgramUnity : NetworkScript, MessageHandler
{
    static public NetworkProgramUnity currentInstance;
    public NetworkScript pfPlayer;
    protected Messenger _messenger;
    bool _stopped = false;

    public bool stopped { get { return _stopped; } }
    public Dictionary<int, NetworkScript> signallersByID { get; set; }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnEnd();

    public new void Send(Packet packet, string server)
    {
        _messenger.Send(server, packet);
    }
    
    void Awake()
    {
        signallersByID = new Dictionary<int, NetworkScript>();
        _messenger = new Messenger(this);
    }

    protected new void Start()
    {
        base.Start();
        DontDestroyOnLoad(gameObject.transform.root);
        currentInstance = this;
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            OnEnd();
            StopAllCoroutines();
            _messenger.Dispose();
        }
        finally
        {
            History.Save();
        }
    }

    protected void Stop()
    {
        _stopped = true;
        Destroy(gameObject);
    }

    void MessageHandler.RPCHandler(TeraTaleNet.RPC rpc)
    {
        NetworkScript script;
        if (signallersByID.TryGetValue(rpc.signallerID, out script))
        {
            if (script.gameObject.activeSelf)
                script.SendMessage(rpc.GetType().Name, rpc);
            else
            {
                SetActive sa = rpc as SetActive;
                if (sa != null)
                    script.SetActive(sa);
                else
                    Debug.Log("A RPC was not arrived. Name:" + rpc.GetType().Name + " DestinationID:" + rpc.signallerID);
            }
        }
        else
        {
            Debug.Log("A RPC was not arrived. Name:" + rpc.GetType().Name + " DestinationID:" + rpc.signallerID);
        }
    }

    public void RegisterSignaller(NetworkScript signaller)
    {
        try
        {
            signallersByID.Add(signaller.networkID, signaller);
        }
        catch(ArgumentException)
        {
            signallersByID[signaller.networkID] = signaller;
            Debug.Log("NID " + signaller.networkID + " already exist.");
        }
    }

    public void UnregisterSignaller(NetworkScript signaller)
    {
        signallersByID.Remove(signaller.networkID);
    }
}