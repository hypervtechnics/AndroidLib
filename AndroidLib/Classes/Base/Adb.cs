using System;
using System.Collections.Generic;
using AndroidLib.Utils;

namespace AndroidLib
{
    public class ADB
    {
        /// <summary>
        /// Returns the status of the adb status
        /// </summary>
        public static bool IsServerRunning
        {
            get
            {
                return Command.IsProcessRunning("adb");
            }
        }

        /// <summary>
        /// Remounts the adb with the given type
        /// </summary>
        /// <param name="type">The mode to remount in</param>
        public static void SetMountType(AdbMountType type)
        {
            if (type == AdbMountType.Root)
                ExecuteAdbCommandWithOutput("root");
            else
                ExecuteAdbCommandWithOutput("unroot");
        }

        /// <summary>
        /// Starts the adb server instance
        /// </summary>
        public static void StartServer()
        {
            Command.RunProcessNoReturn(ResourceManager.adbPrefix, "start-server", true);
        }

        /// <summary>
        /// Kills the current adb server instances
        /// </summary>
        public static void StopServer()
        {
            Command.RunProcessNoReturn(ResourceManager.adbPrefix, "kill-server", true);
        }

        /// <summary>
        /// Executes the adb command on a specific device and returns its output
        /// </summary>
        /// <param name="command">Command to run e.g. "devices"</param>
        /// <param name="device">If this parameter is not set the first device will be selected</param>
        /// <returns>The optimized output of adb</returns>
        public static string ExecuteAdbCommandWithOutput(string command, Device device = null)
        {
            //Check adb status
            if (!IsServerRunning) StartServer();

            //Insert -s parameter if needed
            string cmd = command;
            if (device != null && device.SerialNumber != "") cmd = " -s " + device.SerialNumber + " " + cmd;

            //Run process via Command class
            string output = Command.RunProcessReturnOutput(ResourceManager.adbPrefix, cmd);

            return output;
        }

        /// <summary>
        /// Gets a list of connected devices
        /// </summary>
        /// <returns>List of devices</returns>
        public static List<Device> GetConnectedDevices()
        {
            //Instance the output
            List<Device> devices = new List<Device>();

            //Output: "List of devices attached \r\nXXXXXXXXXXXXXXXX       XXXXXX product:XXXX model:XXXX device:XXXX\r\n\r\n";
            //Split the output for better usage
            string deviceString = ExecuteAdbCommandWithOutput("devices -l", null);
            string[] deviceLines = deviceString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //Check whether a device is connected
            if(deviceLines.Length == 1 && deviceLines[0].Contains("List of devices attached"))
            {
                return devices;
            }

            //Now parse each line
            for(int i = 0; i < deviceLines.Length; i++)
            {
                //If it is debug line: cancel
                if (deviceLines[i].Contains("List of devices attached") || string.IsNullOrWhiteSpace(deviceLines[i]) || deviceLines[i] == "\r\n") continue;

                //Split the device line ("XXXXXXXXXXXXXXXX       XXXXXX product:XXXX model:XXXX device:XXXX")
                string[] parts = deviceLines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                string serialNo = "", model = "", productname = "", name = "";
                DeviceState state;

                //Get serial no
                serialNo = parts[0];

                //Determine state
                switch(parts[1])
                {
                    case "device":
                        state = DeviceState.Online;
                        break;
                    case "offline":
                        state = DeviceState.Offline;
                        break;
                    case "unauthorized":
                        state = DeviceState.Unauthorized;
                        break;
                    case "bootloader":
                        state = DeviceState.Bootloader;
                        break;
                    case "unknown":
                        state = DeviceState.Unknown;
                        break;
                    default:
                        state = DeviceState.Offline;
                        break;
                }

                //Avoid OutOfRange
                if (state == DeviceState.Online || state == DeviceState.Bootloader)
                {
                    //Detect product
                    productname = parts[2].Split(new string[] { ":" }, StringSplitOptions.None)[1];

                    //Detect model
                    model = parts[3].Split(new string[] { ":" }, StringSplitOptions.None)[1];

                    //Detect name
                    name = parts[4].Split(new string[] { ":" }, StringSplitOptions.None)[1];
                }

                //Create, update if requested and add it to result
                Device dev = new Device(serialNo, model, productname, name, state);

                devices.Add(dev);
            }
            
            //Return result
            return devices;
        }

        /// <summary>
        /// Checks if the device is connected
        /// </summary>
        /// <param name="serialNo">The serial number of the device</param>
        /// <returns>Whether the device is connected</returns>
        public static bool IsDeviceConnected(string serialNo)
        {
            //Get list of connected devices and check whether one of them has the given serial number
            List<Device> connectedDevices = GetConnectedDevices();

            if (connectedDevices.Count == 0) return false;

            foreach (Device dev in connectedDevices)
            {
                if (dev.SerialNumber.Equals(serialNo)) return true;
            }

            return false;
        }

        /// <summary>
        /// Wait for a device to connect
        /// </summary>
        public static void WaitForDevice()
        {
            string output = ExecuteAdbCommandWithOutput("wait-for-device");
        }
    }
}
