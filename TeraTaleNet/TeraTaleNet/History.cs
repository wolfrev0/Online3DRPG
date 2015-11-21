using System;
using System.IO;
using System.Diagnostics;

namespace TeraTaleNet
{
    public static class History
    {
        static string _fileName = Process.GetCurrentProcess().ProcessName;
        static StreamWriter writer = new StreamWriter(new FileStream(_fileName + ".history", FileMode.Append));

        static History()
        {
            Log("...............................");
            Log("..........New History..........");
            Log("...............................");
        }

        public static void Log(string message)
        {
            string text = "[" + DateTime.Now + "] " + message;
            Console.WriteLine(text);
            writer.WriteLine(text);
        }

        public static void Save()
        {
            writer.Close();
            writer = new StreamWriter(new FileStream(_fileName + ".history", FileMode.Append));
        }
    }
}