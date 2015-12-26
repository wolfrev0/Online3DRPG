using System.Collections.Generic;
using UnityEngine;

public class NetworkPrefabManager : MonoBehaviour
{
    public NetworkSignaller[] prefabs;

    void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.root);
    }
}