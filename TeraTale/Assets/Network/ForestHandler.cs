using UnityEngine;
using System;
using TeraTaleNet;

public partial class Forest : NetworkScript
{
    public class ForestHandler : MessageHandler
    {
        Forest _body;

        public ForestHandler(Forest forest)
        {
            _body = forest;
        }

        void PlayerJoin(Messenger messenger, string key, PlayerJoin info)
        {
            throw new NotImplementedException();
            _body.players.Add(info.name);
            //NetworkInstantiate
        }
    }
}