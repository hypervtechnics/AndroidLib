using System;
using System.Collections.Generic;
using AndroidLib.Wrapper;
using AndroidLib.Base;
using System.Threading;

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

            Console.WriteLine("Root: " + devices[0].HasRoot);
            
            //Console.WriteLine("------------------------");

            //bool exit = false;

            //while(!exit)
            //{
            //    string input = Console.ReadLine();
            //    if (input == "-") exit = true;
            //    string o = shell.Exec(input);
            //    Console.WriteLine(o.ToString());
            //}

            Console.Write("------------------------");

            Console.ReadLine();

            //PackageManager packages = devices[0].ApplicationManager;

            //Console.WriteLine("Apps installed: " + packages.Packages.Count);

            //foreach(Package pkg in packages.Packages)
            //{
            //    if(!pkg.IsSystemApp) Console.WriteLine("Name: " + pkg.PackageName + " Associated APK: " + pkg.AssociatedFile);
            //}

            //Console.Write("------------------------");

            //Console.ReadLine();

            Backup backup = devices[0].BackupRecover.PrepareBackup(@"C:\Android\testbackup.ab", false, false, false, true, true, true);
            backup.OnBackupCompleted += Backup_OnBackupCompleted;
            backup.OnBackupProgressChanged += Backup_OnBackupProgressChanged;
            backup.Start();

            while(backup.IsRunning)
            {
                Console.WriteLine("Waiting for backup to complete...");
                Thread.Sleep(5000);
            }

            Console.WriteLine("Press any key to exit...");

            Console.ReadLine();

            ADB.StopServer();
        }

        private static void Backup_OnBackupProgressChanged(object sender, Backup.OnBackupProgressChangedArgs e)
        {
            Console.WriteLine("Progress changed: " + e.Size + " Bytes " + (e.WaitingForUserInput ? ", Waiting for user..." : "User accepted!"));
        }

        private static void Backup_OnBackupCompleted(object sender, Backup.OnBackupCompletedArgs e)
        {
            Console.WriteLine("Backup completed! (" + e.Size + " Bytes, " + e.Time.TotalMinutes + " minutes");
            Console.WriteLine("Output:");
            Console.WriteLine(e.Output);
        }
    }
}
