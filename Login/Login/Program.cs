using System;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginServer loginServer = new LoginServer();
            loginServer.Execute();
        }
    }
}