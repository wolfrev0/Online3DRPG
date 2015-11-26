using System.Collections.Generic;
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
}