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

        void ConfirmID(Messenger messenger, string key, ConfirmID confirmID)
        {
            _certificator._confirmID = confirmID.id;
        }

        void LoginAnswer(Messenger messenger, string key, LoginAnswer query)
        {
            Application.LoadLevel(query.world);
        }
    }
}