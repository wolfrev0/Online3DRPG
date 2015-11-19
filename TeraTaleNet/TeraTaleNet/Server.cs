using System;
using System.Collections.Generic;
using System.Threading;

namespace TeraTaleNet
{
    public abstract class Server : IServer
    {
        bool _stopped = false;

        public virtual void Execute()
        {
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
        }

        protected abstract void OnUpdate();

        protected void Stop()
        {
            _stopped = true;
        }
    }
}
