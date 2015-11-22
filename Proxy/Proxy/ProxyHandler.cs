using System.Collections.Generic;
using TeraTaleNet;

namespace Proxy
{
    partial class Proxy : Server
    {
        class ProxyHandler : MessageHandler
        {
            Proxy _server;

            public ProxyHandler(Proxy server)
            {
                _server = server;
            }

            [RPC]
            void LoginResponse(Messenger messenger, string key, Packet packet)
            {
                LoginResponse response = (LoginResponse)packet.body;
                if (response.accepted)
                {
                    var keys = (ICollection<string>)_server._clientMessenger.Keys;
                    if (keys.Contains(response.nickName))
                    {
                        response.accepted = false;
                        response.reason = RejectedReason.LoggedInAlready;

                        _server._confirmMessenger.Send(response.confirmID.ToString(), response);
                    }
                    else
                    {
                        lock (_server._lock)
                        {
                            PacketStream stream = _server._confirmMessenger.Unregister(response.confirmID.ToString());
                            _server._clientMessenger.Register(response.nickName, stream);
                        }
                        _server._messenger.Send("GameServer", new PlayerLogin(response.nickName));

                        _server._clientMessenger.Send(response.nickName, response);
                    }
                }
            }

            [RPC]
            void LoginRequest(Messenger messenger, string key, Packet packet)
            {
                _server._messenger.Send("Login", packet);
            }

            [RPC]
            void PlayerJoin(Messenger messenger, string key, Packet packet)
            {
                PlayerJoin join = (PlayerJoin)packet.body;
                History.Log(join.nickName);
                _server._clientMessenger.Send(join.nickName, packet);
            }
        }
    }
}