using System;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LoginServer server = new LoginServer())
                server.Execute();
        }
    }
}