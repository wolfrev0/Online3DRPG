using UnityEngine;
using TeraTaleNet;

public class GameServerHandler : MessageHandler
{
    void PlayerLogin(Messenger messenger, string key, PlayerLogin login)
    {
        messenger.Send("Database", new PlayerInfoRequest(login.nickName));

        PlayerInfoResponse info = (PlayerInfoResponse)messenger.ReceiveSync("Database").body;
        messenger.Send("Proxy", new PlayerJoin(info.nickName, info.world));
    }
}