using System;
using System.Collections.Generic;
using System.Text;

namespace TM.Digital.Services
{
    public static class Logger
    {
        public static void Log(string playerName, string message)
        {
            Console.WriteLine($"{playerName} - {message}");
        }
    }
}
