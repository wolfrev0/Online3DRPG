namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyServer server = new ProxyServer();
            server.Execute();
        }
    }
}