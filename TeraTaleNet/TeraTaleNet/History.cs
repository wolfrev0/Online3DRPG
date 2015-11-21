using System;
using System.IO;
using System.Diagnostics;

namespace TeraTaleNet
{
    public static class History
    {
        static string _fileName = Process.GetCurrentProcess().ProcessName;
        static StreamWriter _writer = new StreamWriter(new FileStream(_fileName + ".history", FileMode.Append));
        static object _locker = new object();

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
            lock (_locker)
            {
                if (_writer == null)
                    _writer = new StreamWriter(new FileStream(_fileName + ".history", FileMode.Append));
                _writer.WriteLine(text);
            }
        }

        public static void Save()
        {
            lock (_locker)
            {
                if (_writer != null)
                    _writer.Close();
                _writer = null;
            }
        }
    }
}