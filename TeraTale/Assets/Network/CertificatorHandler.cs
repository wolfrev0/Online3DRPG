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

        [TeraTaleNet.RPC]
        void LoginResponse(Messenger messenger, string key, Packet packet)
        {
            LoginResponse response = (LoginResponse)packet.body;
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

        [TeraTaleNet.RPC]
        void ConfirmID(Messenger messenger, string key, Packet packet)
        {
            _certificator._confirmID = ((ConfirmID)packet.body).id;
        }
    }
}