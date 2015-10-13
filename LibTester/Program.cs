﻿using System;
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

            Console.Read();

            Device dev = devices[0];

            Console.Read();

            Console.WriteLine(dev.SerialNumber);

            AdbPushPullResult result = dev.Push(@"C:\Android\WA", "/sdcard/TESTER");

            Console.WriteLine(result.Files.Count);

            Console.ReadLine();

            ADB.StopServer();
        }
    }
}
