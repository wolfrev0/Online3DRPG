﻿using System;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginServer server = new LoginServer();
            server.Execute();
        }
    }
}