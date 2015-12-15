using UnityEngine;

public class NetworkSignaller : MonoBehaviour
{
    NetworkProgramUnity _script;
    int _networkID = -1;
    string _owner = null;

    public void Initialize(int networID, string owner)
    {
        _networkID = networID;
        _owner = owner;
    }
}