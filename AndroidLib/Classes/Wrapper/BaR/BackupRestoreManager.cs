using AndroidLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidLib.Wrapper
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
        /// <param name="asyncProcess">Whether to block thread until backup is completed</param>
        /// <param name="packages">Optionally the list of packages to backup (Note: backupAll has to be false)</param>
        /// <returns>An object handling the Backup process</returns>
        public Backup MakeBackup(string filename, bool includeApk, bool includeObb, bool includeInternal, bool backupAll, bool includeSystemApps, bool asyncProcess, List<string> packages = null)
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

            if(packages != null && !backupAll)
            {
                foreach(string packagename in packages)
                {
                    command += " " + packagename;
                }
            }

            //Create object and block if neccessary
            Backup backup = new Backup(command, mDevice, filename);

            if(!asyncProcess)
            {

                EventWaitHandle waiter = new ManualResetEvent(false);
                backup.OnBackupCompleted += ((object sender, Backup.OnBackupCompletedArgs Eventargs) => waiter.Set());
                backup.Start();
                waiter.WaitOne();
                backup.Dispose();
                return backup;
            }else
            {
                return backup;
            }
        }
    }
}
