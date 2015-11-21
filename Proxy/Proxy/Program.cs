namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ProxyServer server = new ProxyServer())
                server.Execute();
        }
    }
}