using TeraTaleNet;

class LoginHandler : MessageHandler
{
    void LoginRequest(Messenger messenger, string key, Packet packet)
    {
        messenger.Send("Database", packet);
        messenger.Send("Proxy", messenger.ReceiveSync("Database"));
    }
}