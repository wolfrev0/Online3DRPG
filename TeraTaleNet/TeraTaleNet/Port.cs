using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public enum Port : ushort
    {
        Client = 9852,
        Proxy,
        LoginForProxy,
        LoginForGameServer,
        Database,
        GameServer, 
    }
}
