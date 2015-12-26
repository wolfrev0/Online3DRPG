using System;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Login server = new Login())
                server.Execute();
        }
    }
}