using System;
using LoboNet;
using UnityEngine;

namespace TeraTaleNet
{
    public abstract class NetworkScript : MonoBehaviour
    {
        bool _stopped = false;

        public bool stopped { get { return _stopped; } }

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
            try
            {
                OnEnd();
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
    }
}