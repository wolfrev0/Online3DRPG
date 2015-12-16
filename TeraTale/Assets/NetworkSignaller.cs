using UnityEngine;

public class NetworkSignaller : MonoBehaviour
{
    NetworkProgramUnity _script;
    int _networkID = -1;
    string _owner = null;

    public void Initialize(int networID, string owner)
    {
        var programs = FindObjectsOfType<NetworkProgramUnity>();
        foreach(var s in programs)
        {
            if (s.enabled)
                _script = s;
        }
        _networkID = networID;
        _owner = owner;
    }

    public bool isMine { get { return _script.userName == _owner; } }
}