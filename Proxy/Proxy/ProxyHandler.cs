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
            
            void LoginResponse(Messenger messenger, string key, LoginResponse response)
            {
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
            
            void LoginRequest(Messenger messenger, string key, Body packet)
            {
                _server._messenger.Send("Login", packet);
            }
            
            void PlayerJoin(Messenger messenger, string key, PlayerJoin join)
            {
                History.Log(join.nickName);
                _server._clientMessenger.Send(join.nickName, join);
            }
        }
    }
}