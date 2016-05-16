using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidLib.Interaction
{
    public class BackupRestoreManager
    {
        private Device mDevice;

        internal BackupRestoreManager(Device device)
        {
            mDevice = device;
        }

        /// <summary>
        /// Creates an Backup instance which manages the backup process
        /// </summary>
        /// <param name="filename">The filename of the file the backup will be saved to</param>
        /// <param name="includeApk">Whether the associated apk file of an app should be saved</param>
        /// <param name="includeObb">Whether to backup the associated apk expansion file of the app</param>
        /// <param name="includeInternal">Whether to include sd card contents</param>
        /// <param name="backupAll">Whether to backup all applications</param>
        /// <param name="includeSystemApps">Whether to include system apps too</param>
        /// <param name="packages">Optionally the list of packages to backup (Note: backupAll has to be false)</param>
        /// <param name="interval">The interval in which to check for changes</param>
        /// <returns>An object handling the Backup process</returns>
        public Backup PrepareBackup(string filename, bool includeApk, bool includeObb, bool includeInternal, bool backupAll, bool includeSystemApps, List<string> packages = null, CheckInterval interval = CheckInterval.Middle)
        {
            //Build cmd string
            string command = "backup -f \"" + filename + "\" ";

            if (includeApk) command += "-apk ";
            else command += "-noapk ";

            if (includeObb) command += "-obb ";
            else command += "-noobb";

            if (includeInternal) command += "-shared ";
            else command += "-noshared ";

            if (backupAll) command += "-all ";

            if (includeSystemApps) command += "-system";
            else command += "-nosystem";

            if (packages != null && !backupAll)
            {
                foreach (string packagename in packages)
                {
                    command += " " + packagename;
                }
            }

            //Create object
            Backup backup = new Backup(command, mDevice, filename, (int) interval);

            return backup;
        }

        /// <summary>
        /// Does the backup directly and blocks the thread until finish
        /// </summary>
        /// <param name="filename">The filename of the file the backup will be saved to</param>
        /// <param name="includeApk">Whether the associated apk file of an app should be saved</param>
        /// <param name="includeObb">Whether to backup the associated apk expansion file of the app</param>
        /// <param name="includeInternal">Whether to include sd card contents</param>
        /// <param name="backupAll">Whether to backup all applications</param>
        /// <param name="includeSystemApps">Whether to include system apps too</param>
        /// <param name="packages">Optionally the list of packages to backup (Note: backupAll has to be false)</param>
        /// <param name="interval">The interval in which to check for changes</param>
        public void DoBackup(string filename, bool includeApk, bool includeObb, bool includeInternal, bool backupAll, bool includeSystemApps, List<string> packages = null, CheckInterval interval = CheckInterval.Middle)
        {
            //Create instance of backup
            Backup backup = this.PrepareBackup(filename, includeApk, includeObb, includeObb, backupAll, includeSystemApps, packages, interval);

            //Block until it finished
            EventWaitHandle waiter = new ManualResetEvent(false);
            backup.OnBackupCompleted += ((object sender, Backup.OnBackupCompletedArgs Eventargs) => waiter.Set());
            backup.Start();
            waiter.WaitOne();
            backup.Dispose();
        }
        
        /// <summary>
        /// Does the restore of the given file
        /// </summary>
        /// <param name="filename">The .ab file to restore</param>
        /// <returns></returns>
        public String DoRestore(string filename)
        {
            return ADB.ExecuteAdbCommandWithOutput("restore \"" + filename + "\"", mDevice);
        }
    }
}
