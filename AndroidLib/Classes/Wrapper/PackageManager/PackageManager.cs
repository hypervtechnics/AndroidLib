using AndroidLib.Base;
using AndroidLib.Results;
using System;
using System.Collections.Generic;

namespace AndroidLib.Wrapper
{
    public class PackageManager
    {
        private List<Package> mPackages;
        private InstallLocationType mInstallLocation;

        internal Device mDevice;

        internal PackageManager(Device dev)
        {
            mDevice = dev;
            mPackages = new List<Package>();
            this.Update();
        }
        
        /// <summary>
        /// A list containing all packages installed with its information
        /// </summary>
        public List<Package> Packages
        {
            get
            {
                return mPackages;
            }
        }

        /// <summary>
        /// Gets and sets (if rooted) the install location
        /// </summary>
        public InstallLocationType InstallLocation
        {
            get
            {
                return mInstallLocation;
            }
            set
            {
                //Only possible if device has root
                if(mDevice.HasRoot)
                {
                    mDevice.CommandShell.Exec("pm set-install-location " + value, true);
                }
            }
        }

        /// <summary>
        /// This methods refreshes all values
        /// </summary>
        public void Update()
        {
            //Update install location
            { 
                String output = mDevice.CommandShell.Exec("pm get-install-location");

                if (output.Contains("0")) mInstallLocation = InstallLocationType.Auto;
                else if (output.Contains("1")) mInstallLocation = InstallLocationType.Internal;
                else if (output.Contains("2")) mInstallLocation = InstallLocationType.External;
            }
        }

        /// <summary>
        /// Installs the given APK file on the device
        /// </summary>
        /// <param name="apkPath">The path of the apk file</param>
        /// <param name="forwardLock">Forward lock application</param>
        /// <param name="replaceExisting">Replace existing application</param>
        /// <param name="allowTest">Allow test packages</param>
        /// <param name="installOnSd">Install application on sdcard</param>
        /// <param name="allowDowngrade">Allow version code downgrade</param>
        /// <param name="grantAllPermissions">Grant all runtime permissions</param>
        /// <returns>An object containing the information about the installation process</returns>
        public AdbInstallResult InstallApk(string apkPath, Boolean forwardLock, Boolean replaceExisting, Boolean allowTest, Boolean installOnSd, Boolean allowDowngrade, Boolean grantAllPermissions)
        {
            //Initialize empty string
            string command = "install ";

            //Now form it
            if (forwardLock) command += "-l ";
            if (replaceExisting) command += "-r ";
            if (allowTest) command += "-t ";
            if (installOnSd) command += "-s ";
            if (allowDowngrade) command += "-d ";
            if (grantAllPermissions) command += "-g ";
            command += "\"" + apkPath + "\"";

            string output = ADB.ExecuteAdbCommandWithOutput(command, mDevice);

            //Return it
            return new AdbInstallResult(output.Contains("Success"), output);
        }

        /// <summary>
        /// Uninstalls an app from the device
        /// </summary>
        /// <param name="packageName">The package name of the application</param>
        /// <param name="keepData">If the data in /data/data/<pkgname>/ should be kept</param>
        /// <returns>If the uninstalling of the package was successful</returns>
        public Boolean UninstallApp(string packageName, Boolean keepData = false)
        {
            string command = "";

            if (keepData) command += "uninstall ";
            else command += "shell pm uninstall -k ";

            command += packageName;

            return ADB.ExecuteAdbCommandWithOutput(command, mDevice).Contains("Success");
        }
    }
}
