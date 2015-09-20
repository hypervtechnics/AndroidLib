using AndroidLib.Adb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(Adb.ExecuteAdbCommandWithOutput("devices", null, false));
            Console.Write("FINISHED!");
            Console.Read();
        }
    }
}
