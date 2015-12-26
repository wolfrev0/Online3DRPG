using System;
using TeraTaleNet;

class LoginHandler : MessageHandler
{
    void LoginQuery(Messenger messenger, string key, LoginQuery query)
    {
        messenger.Send("Database", query);
    }

    void LoginAnswer(Messenger messenger, string key, LoginAnswer query)
    {
        messenger.Send("Proxy", query);
    }

    void MessageHandler.RPCHandler(RPC rpc)
    {
        throw new NotImplementedException();
    }
}