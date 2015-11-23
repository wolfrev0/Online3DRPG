using UnityEngine;
using TeraTaleNet;

public class NetworkHandler : MessageHandler
{
    Network _net;

    public NetworkHandler(Network net)
    {
        _net = net;
    }
    
    void PlayerJoin(Messenger messenger, string key, PlayerJoin join)
    {
        Player player = Object.Instantiate(_net.pfPlayer);
        player.gameObject.name = join.nickName;
    }
}