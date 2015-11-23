using UnityEngine;
using TeraTaleNet;

public partial class Certificator : UnityServer
{
    class CertificatorHandler : MessageHandler
    {
        Certificator _certificator;

        public CertificatorHandler(Certificator certificator)
        {
           _certificator = certificator;
        }
        
        void LoginResponse(Messenger messenger, string key, LoginResponse response)
        {
            if (response.accepted)
            {
                var net = FindObjectOfType<Network>();
                lock (_certificator._locker)
                    net.stream = messenger.Unregister("Proxy");
                net.enabled = true;
                Application.LoadLevel("Town");
            }
            else
            {

            }
        }
        
        void ConfirmID(Messenger messenger, string key, ConfirmID confirmID)
        {
            _certificator._confirmID = confirmID.id;
        }
    }
}