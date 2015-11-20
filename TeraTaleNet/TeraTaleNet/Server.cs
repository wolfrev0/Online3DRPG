using System;
using System.Threading;

namespace TeraTaleNet
{
    public abstract class Server : IServer
    {
        bool _stopped = false;

        public void Execute()
        {
            OnStart();
            try
            {
                while (_stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            OnEnd();
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        protected void Stop()
        {
            _stopped = true;
        }
    }
}
