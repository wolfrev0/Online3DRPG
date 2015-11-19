namespace TeraTaleNet
{
    public enum Port : ushort
    {
        Client = 9852,
        Proxy,
        LoginForProxy,
        LoginForGameServer,
        DatabaseForLogin,
        DatabaseForGameServer,
        GameServer,
    }
}