namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Proxy server = new Proxy())
                server.Execute();
        }
    }
}