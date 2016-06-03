using System;
using System.Collections.Generic;
using AndroidLib.Interaction;
using System.Threading;
using AndroidLib;
using AndroidLib.Results;

namespace AndroidLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            List<Device> devices = ADB.GetConnectedDevices();
            Console.WriteLine("Devices connected: " + devices.Count);
            Console.WriteLine("------------------------");
            Console.ReadKey();
            //Device d = devices[0];
            /////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////Working area///////////////////////////////////////////
            
            /////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            ADB.StopServer();
        }
    }
}
