using UnityEngine;
using TeraTaleNet;

public class GlobalOption : MonoBehaviour
{
    void Awake()
    {
        Application.logMessageReceived += (string logString, string stackTrace, LogType type) =>
        {
            History.Log(logString);
            History.Log(stackTrace);
        };
    }
}