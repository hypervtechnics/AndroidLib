using System;
using System.Collections.Generic;
using AndroidLib.Results;
using AndroidLib.Base;

namespace LibTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (!Adb.ServerIsRunning) Adb.StartServer();
            //Console.WriteLine("Started!");
            //Console.Write(Adb.ExecuteAdbCommandWithOutput("devices -l", null, false));
            //Console.Write("FINISHED!");
            //Console.Read();
            //Adb.StopServer();
            //Console.WriteLine("Stopped!");

            List<Device> devices = ADB.GetConnectedDevices();

            Console.WriteLine(devices.Count);

            Shell shell = devices[0].CommandShell;

            Console.WriteLine(devices[0].SerialNumber);

            Console.ReadLine();

            Console.WriteLine("------------------------");

            Boolean exit = false;

            while(!exit)
            {
                string input = Console.ReadLine();
                if (input == "ExIt") exit = true;
                List<string> o = shell.RunCommand(input);
                Console.WriteLine(o.ToString());
            }

            Console.WriteLine("------------------------");

            Console.WriteLine("Press any key to exit...");

            Console.ReadLine();

            ADB.StopServer();
        }
    }
}
