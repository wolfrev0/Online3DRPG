using UnityEngine;
using TeraTaleNet;

public abstract class UnityServer : MonoBehaviour, IServer
{
    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnEnd();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void OnDestroy()
    {
        OnEnd();
    }

    protected void Stop()
    {
        Destroy(gameObject);
    }
}