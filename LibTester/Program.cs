using System;
using System.Collections.Generic;
using AndroidLib.Wrapper;
using System.Threading;
using AndroidLib;

namespace LibTester
{
    class Program
    {
        static void Main(string[] args)
        { 
            List<Device> devices = ADB.GetConnectedDevices();

            Console.WriteLine("Devices connected: " + devices.Count);
            
            Console.WriteLine("------------------------");

            Console.ReadLine();

            Shell shell = devices[0].CommandShell;

            Console.WriteLine("Serial number: " + devices[0].SerialNumber);

            Console.WriteLine("Install location: " + devices[0].ApplicationManager.InstallLocation.ToString());

            Console.WriteLine("Root: " + devices[0].HasRoot);

            Console.WriteLine("------------------------");

            Console.ReadLine();

            Console.WriteLine("Press any key to exit...");

            Console.ReadLine();

            ADB.StopServer();
        }
    }
}
