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

            Console.WriteLine("Devices connected: " + devices.Count);

            Shell shell = devices[0].CommandShell;

            Console.WriteLine("Serial number: " + devices[0].SerialNumber);

            Console.WriteLine("Install location: " + devices[0].ApplicationManager.InstallLocation.ToString());

            Console.Write("Root: " + devices[0].HasRoot);

            Console.ReadLine();

            Console.WriteLine("------------------------");

            Boolean exit = false;

            while(!exit)
            {
                string input = Console.ReadLine();
                if (input == "ExIt") exit = true;
                string o = shell.Exec(input);
                Console.WriteLine(o.ToString());
            }

            Console.WriteLine("------------------------");
            
            Console.WriteLine("Press any key to exit...");

            Console.ReadLine();

            ADB.StopServer();
        }
    }
}
