using UnityEngine;
using TeraTaleNet;

public class NetworkHandler : MessageHandler
{
    Network _net;

    public NetworkHandler(Network net)
    {
        _net = net;
    }
    
    void PlayerJoin(Messenger messenger, string key, Packet packet)
    {
        PlayerJoin join = (PlayerJoin)packet.body;
        Player player = Object.Instantiate(_net.pfPlayer);
        player.gameObject.name = join.nickName;
    }
}