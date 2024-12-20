using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakaEngine.Core.Helpers
{
    public static class Debug
    {
        public static void Log(string Message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message);
        }
        public static void Warning(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: " + Message);
        }
        public static void Error(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + Message);
        }
    }
}
