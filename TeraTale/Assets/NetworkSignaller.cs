using UnityEngine;
using System.Collections;

public class NetworkSignaller : MonoBehaviour
{
    public int _networkID = 0;
    public string _owner = null;

    bool registered = false;

    public bool isMine { get { return NetworkProgramUnity.currentInstance.userName == _owner; } }

    IEnumerator Start()
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

    public void SendRPC(TeraTaleNet.RPC rpc)
    {
        rpc.signallerID = _networkID;
        rpc.sender = NetworkProgramUnity.currentInstance.userName;
        NetworkProgramUnity.currentInstance.Send(rpc);
    }
}