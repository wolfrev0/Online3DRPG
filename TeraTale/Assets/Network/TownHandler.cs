using UnityEngine;
using System;
using TeraTaleNet;

public partial class Town : NetworkScript
{
    public class TownHandler : MessageHandler
    {
        Town _body;

        public TownHandler(Town town)
        {
            _body = town;
        }

        void PlayerJoin(Messenger messenger, string key, PlayerJoin info)
        {
            throw new NotImplementedException();
            //Add,
            //NetworkInstantiate
        }
    }
}