using System;
using System.Threading;

namespace TeraTaleNet
{
    public abstract class NetworkProgram
    {
        bool _stopped = false;

        public bool stopped { get { return _stopped; } }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        public void Execute()
        {
            try
            {
                OnStart();
                while (_stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                History.Log(e.ToString());
            }
            finally
            {
                History.Save();
                OnEnd();
            }
        }

        protected void Stop()
        {
            _stopped = true;
        }
    }
}
