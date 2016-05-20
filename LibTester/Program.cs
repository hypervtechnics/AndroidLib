using System;
using System.Collections.Generic;
using AndroidLib.Interaction;
using System.Threading;
using AndroidLib;
using AndroidLib.Results;

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
            Device d = devices[0];
            /////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////Working area///////////////////////////////////////////
            FileSystemManager fsm = d.FileSystem;
            InteractionResult<List<FileSystemObject>> res = fsm.GetFilesFromDirRecursive("/sdcard/");
            Console.WriteLine(res.WasSuccessful.ToString());
            List<FileSystemObject> fsos = res.Result;
            foreach(FileSystemObject fso in fsos)
            {
                Console.WriteLine(fso.Path + " | " + fso.Permissions.GetAsNumber() + " | " + fso.Size + " | " + fso.IsDirectory);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            ADB.StopServer();
        }
    }
}
