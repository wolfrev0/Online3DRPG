using UnityEngine;
using TeraTaleNet;

public class GameServerHandler : MessageHandler
{
    [TeraTaleNet.RPC]
    void PlayerLogin(Messenger messenger, string key, Packet packet)
    {
        PlayerLogin login = (PlayerLogin)packet.body;
        messenger.Send("Database", new PlayerInfoRequest(login.nickName));

        PlayerInfoResponse info = (PlayerInfoResponse)messenger.ReceiveSync("Database").body;
        messenger.Send("Proxy", new PlayerJoin(info.nickName, info.world));
    }
}