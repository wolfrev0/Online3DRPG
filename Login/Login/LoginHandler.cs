using TeraTaleNet;

class LoginHandler : MessageHandler
{
    void LoginRequest(Messenger messenger, string key, Body body)
    {
        messenger.Send("Database", body);
        messenger.Send("Proxy", messenger.ReceiveSync("Database"));
    }
}