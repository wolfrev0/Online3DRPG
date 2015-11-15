using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public enum TargetPort : ushort
    {
        Client = 9852,
        Proxy,
        Login,
        Database,
        GameServer, 
    }
}
