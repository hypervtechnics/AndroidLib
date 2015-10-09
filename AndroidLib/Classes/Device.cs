﻿using AndroidLib.Adb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib
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
        internal Device(String serialNo, String model, String productName, String name, DeviceState state)
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

        private String mSerialNumber;
        private String mModel;
        private String mProductName;
        private DeviceState mConnectionStatus;
        private String mName;

        #endregion

        #region Public Fields

        /// <summary>
        /// Returns the serial number of the device
        /// </summary>
        public String SerialNumber
        {
            get
            {
                return mSerialNumber;
            }
        }

        /// <summary>
        /// Returns the model number of the device
        /// </summary>
        public String Model
        {
            get
            {
                return mModel;
            }
        }

        /// <summary>
        /// Returns the product name of the device
        /// </summary>
        public String ProductName
        {
            get
            {
                return mProductName;
            }
        }

        /// <summary>
        /// Returns the name of the device
        /// </summary>
        public String Name
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
        /// Pulls the file or directory to the device
        /// </summary>
        /// <param name="pathOnDevice"></param>
        /// <param name="pathOnComputer"></param>
        /// <returns></returns>
        public AdbPushPullResult Pull(String pathOnDevice, String pathOnComputer)
        {
            //Do it and get its output for further analysis
            String output = Adb.Adb.ExecuteAdbCommandWithOutput("pull \"" + pathOnDevice + "\" \"" + pathOnComputer + "\"", this);

            //Split it into the lines
            String[] lines = output.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //Store values for creation of the result
            int transferrate = 0;
            Boolean success = false, singlefile = true;
            long size = 0L;
            Double secondsneeded = 0.0;
            Dictionary<String, String> files = new Dictionary<String, String>();
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

                //Add 
                for(int i = 1; i < lines.Length - 2; i++)
                {
                    if (!lines[i].StartsWith("pull:")) continue;

                    String tmpLine = lines[i].After("pull:");
                    String[] tmpSplit = tmpLine.Split(new String[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                    files.Add(tmpSplit[0].Trim(), tmpSplit[1].Trim());
                }
            }
            else
            {
                files.Add(pathOnDevice, pathOnComputer);
            }

            //Parse last line
            String lastLine = lines[lines.Length - 1];
            transferrate = int.Parse(lastLine.Before(" "));
            size = long.Parse(lastLine.Between(" (", " bytes"));
            secondsneeded = Double.Parse(lastLine.Between("bytes in ", "s"), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

            //Finally return the object
            return new AdbPushPullResult(transferrate, success, singlefile, size, files, secondsneeded, output, error);
        }

        public String Push(String pathOnComputer, String pathOnDevice)
        {
            String output = Adb.Adb.ExecuteAdbCommandWithOutput("push \"" + pathOnComputer + "\" \"" + pathOnDevice + "\"", this);

            return output;
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
