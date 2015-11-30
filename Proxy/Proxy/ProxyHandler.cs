using System.Collections.Generic;
using TeraTaleNet;

namespace Proxy
{
    partial class Proxy
    {
        class ProxyHandler : MessageHandler
        {
            Proxy _server;

            public ProxyHandler(Proxy server)
            {
                _server = server;
            }

            void LoginQuery(Messenger messenger, string key, LoginQuery query)
            {
                _server._messenger.Send("Login", query);
            }

            void LoginAnswer(Messenger messenger, string key, LoginAnswer query)
            {
                if (query.accepted)
                {
                    var keys = (ICollection<string>)_server._clientMessenger.Keys;
                    if (keys.Contains(query.name))
                    {
                        query.accepted = false;
                        _server._confirmMessenger.Send(query.confirmID.ToString(), query);
                    }
                    else
                    {
                        lock (_server._lock)
                        {
                            PacketStream stream = _server._confirmMessenger.Unregister(query.confirmID.ToString());
                            _server._clientMessenger.Register(query.name, stream);
                        }
                        _server._clientMessenger.Send(query.name, query);
                        //_server._messenger.Send(query.world, new PlayerJoin());
                    }
                }
                else
                {
                    _server._confirmMessenger.Send(query.confirmID.ToString(), query);
                }
            }
        }
    }
}