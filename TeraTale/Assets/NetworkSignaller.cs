using UnityEngine;

public class NetworkSignaller : MonoBehaviour
{
    public int _networkID = 0;
    public string _owner = null;

    public bool isMine { get { return NetworkProgramUnity.currentInstance.userName == _owner; } }

    void Start()
    {
        NetworkProgramUnity.currentInstance.RegisterSignaller(this);
    }

    public void SendRPC(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = _networkID;
        rpc.sender = NetworkProgramUnity.currentInstance.userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }
}