using AndroidLib.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Adb
{
    public class Device
    {
        #region Constructors

        /// <summary>
        /// Creates a device with the specific serial number
        /// </summary>
        /// <param name="serialNo">The serial number of the device as a String</param>
        /// <param name="model">The model of the device</param>
        /// <param name="productName">The product name of the device</param>
        /// <param name="state">The state of the device</param>
        internal Device(string serialNo, string model, string productName, string name, DeviceState state)
        {
            mSerialNumber = serialNo;
            mProductName = productName;
            mModel = model;
            mConnectionStatus = state;
            mName = name;
            this.updateInfo();
        }
        
        #endregion

        #region Private Fields

        private string mSerialNumber;
        private string mModel;
        private string mProductName;
        private DeviceState mConnectionStatus;
        private string mName;

        #endregion

        #region Public Fields

        /// <summary>
        /// Returns the serial number of the device
        /// </summary>
        public string SerialNumber
        {
            get
            {
                return mSerialNumber;
            }
        }

        /// <summary>
        /// Returns the model number of the device
        /// </summary>
        public string Model
        {
            get
            {
                return mModel;
            }
        }

        /// <summary>
        /// Returns the product name of the device
        /// </summary>
        public string ProductName
        {
            get
            {
                return mProductName;
            }
        }

        /// <summary>
        /// Returns the name of the device
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
        }

        /// <summary>
        /// Returns the state of the device
        /// </summary>
        public DeviceState ConnectionStatus
        {
            get
            {
                return mConnectionStatus;
            }
        }

        #endregion

        #region Private Methods

        internal void updateInfo()
        {
            //TODO
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Updates all values of this device
        /// </summary>
        public void Update()
        {
            this.updateInfo();
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

            string output = Adb.ExecuteAdbCommandWithOutput(command, this);

            //Return it
            return new AdbInstallResult(output.Contains("Success"), output);
        }

        /// <summary>
        /// Pulls the file or directory from the device
        /// </summary>
        /// <param name="pathOnDevice">The path of the file or directory on the device</param>
        /// <param name="pathOnComputer">The target path (like 'C:\Android')</param>
        /// <returns>The AdbPushPullResult containing the infos about the transfer</returns>
        public AdbPushPullResult Pull(string pathOnDevice, string pathOnComputer)
        {
            //Do it and get its output for further analysis
            string output = Adb.ExecuteAdbCommandWithOutput("pull \"" + pathOnDevice + "\" \"" + pathOnComputer + "\"", this);

            //Split it into the lines
            string[] lines = output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //Store values for creation of the result
            int transferrate = 0;
            Boolean success = false, singlefile = true;
            long size = 0L;
            Double secondsneeded = 0.0;
            Dictionary<string, string> files = new Dictionary<string, string>();
            ErrorType error = ErrorType.None;

            //Check whether it was successful and if not abort it
            if (lines[0].StartsWith("error:"))
            {
                error = ErrorType.Unknown;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }
            else if(lines[0].StartsWith("cannot create"))
            {
                error = ErrorType.NoSuchFileOrDirectory;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }
            else if(lines[0].StartsWith("remote object"))
            {
                error = ErrorType.RemoteObjectNotFound;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }

            //Seems successful
            success = true;

            //Indicate whether it was a single file or multiple files
            if(lines[0].StartsWith("pull: building file list...") && lines.Length >= 5)
            {
                singlefile = false;

                //Add them to the dictionary
                for (int i = 1; i < lines.Length - 2; i++)
                {
                    if (!lines[i].StartsWith("pull:")) continue;

                    string tmpLine = lines[i].After("pull:");
                    string[] tmpSplit = tmpLine.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                    files.Add(tmpSplit[0].Trim(), tmpSplit[1].Trim());
                }
            }
            else
            {
                files.Add(pathOnDevice, pathOnComputer);
            }

            //Parse last line
            string lastLine = lines[lines.Length - 1];
            transferrate = int.Parse(lastLine.Before(" "));
            size = long.Parse(lastLine.Between(" (", " bytes"));
            secondsneeded = Double.Parse(lastLine.Between("bytes in ", "s"), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

            //Finally return the object
            return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
        }

        /// <summary>
        /// Pushs the file or directory to the device
        /// </summary>
        /// <param name="pathOnComputer">The path of the file or directory on the computer</param>
        /// <param name="pathOnDevice">The target path</param>
        /// <returns>The AdbPushPullResult containing the infos about the transfer</returns>
        public AdbPushPullResult Push(string pathOnComputer, string pathOnDevice)
        {
            //Do it and get its output for further analysis
            string output = Adb.ExecuteAdbCommandWithOutput("push \"" + pathOnComputer + "\" \"" + pathOnDevice + "\"", this);

            //Split it into the lines
            string[] lines = output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //Store values for creation of the result
            int transferrate = 0;
            Boolean success = false, singlefile = true;
            long size = 0L;
            Double secondsneeded = 0.0;
            Dictionary<string, string> files = new Dictionary<string, string>();
            ErrorType error = ErrorType.None;

            //Check whether it was successful and if not abort it
            if (lines[0].StartsWith("error:"))
            {
                error = ErrorType.Unknown;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }
            else if (lines[0].StartsWith("cannot create"))
            {
                error = ErrorType.NoSuchFileOrDirectory;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }
            else if (lines[0].StartsWith("remote object"))
            {
                error = ErrorType.RemoteObjectNotFound;
                return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
            }

            //Seems successful
            success = true;

            //Indicate whether it was a single file or multiple files
            if (lines.Length >= 5)
            {
                singlefile = false;

                //Add them to the dictionary
                for (int i = 0; i < lines.Length - 2; i++)
                {
                    if (!lines[i].StartsWith("push:")) continue;

                    string tmpLine = lines[i].After("push:");
                    string[] tmpSplit = tmpLine.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                    files.Add(tmpSplit[0].Trim(), tmpSplit[1].Trim());
                }
            }
            else
            {
                files.Add(pathOnDevice, pathOnComputer);
            }

            //Parse last line
            string lastLine = lines[lines.Length - 1];
            transferrate = int.Parse(lastLine.Before(" "));
            size = long.Parse(lastLine.Between(" (", " bytes"));
            secondsneeded = Double.Parse(lastLine.Between("bytes in ", "s"), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

            //Finally return the object
            return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
        }

        #endregion
    }

    #region State of devices

    public enum DeviceState
    {
        Unknown,
        Online,
        Offline,
        Bootloader,
        Unauthorized
    }

    #endregion
}