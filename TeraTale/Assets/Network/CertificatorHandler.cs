using UnityEngine;
using TeraTaleNet;

public partial class Certificator : NetworkScript
{
    class CertificatorHandler : MessageHandler
    {
        Certificator _body;

        public CertificatorHandler(Certificator certificator)
        {
           _body = certificator;
        }

        void ConfirmID(Messenger messenger, string key, ConfirmID confirmID)
        {
            _body._confirmID = confirmID.id;
        }

        void LoginAnswer(Messenger messenger, string key, LoginAnswer query)
        {
            Application.LoadLevel(query.world);
        }
    }
}