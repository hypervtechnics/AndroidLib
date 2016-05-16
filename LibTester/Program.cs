using System;
using System.Collections.Generic;
using AndroidLib.Interaction;
using System.Threading;
using AndroidLib;

namespace LibTester
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
            //d.BackupRestoreManager.DoBackup("C:\\Android\\testbackup.ab", false, false, false, false, false, new List<string>(new string[] { "com.whatsapp" }));
            BackupFile b = BackupFile.FromFile("C:\\Android\\testbackup2.ab");
            Console.WriteLine(b.SetPassword("1234567890"));
            /////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            ADB.StopServer();
        }
    }
}
