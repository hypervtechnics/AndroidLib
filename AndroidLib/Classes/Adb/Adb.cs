using System;
using System.Collections.Generic;
using AndroidLib.Utils;
using System.Text;

namespace AndroidLib.Adb
{
    public class Adb
    {
        #region Public Fields

        /// <summary>
        /// Returns the status of the adb status
        /// </summary>
        public static Boolean ServerIsRunning
        {
            get
            {
                return Command.IsProcessRunning("adb");
            }
        }

        #endregion

        #region Public Methods

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
        /// <param name="trimOutput">Remove stuff like "*daemon started successfully*" from output</param>
        /// <returns>The optimized output of adb</returns>
        public static String ExecuteAdbCommandWithOutput(String command, Device device = null, Boolean trimOutput = true)
        {
            //Check adb status
            if (!ServerIsRunning) StartServer();

            //Insert -s parameter if needed
            String cmd = command;
            if (device != null && device.SerialNumber != "") cmd = "-s " + device.SerialNumber + "" + cmd;

            //Run process via Command class
            String output = Command.RunProcessReturnOutput(ResourceManager.adbPrefix, cmd);

            //Trim if neccessary
            if (trimOutput)
                return RemoveAdbDebugStuff(output);
            else
                return output;
        }

        /// <summary>
        /// Gets a list of connected devices
        /// </summary>
        /// <param name="updateThemDirectly">Call the update method of the devices directly</param>
        /// <returns>List of devices</returns>
        public static List<Device> GetConnectedDevices(Boolean updateThemDirectly = false)
        {
            //Instance the output
            List<Device> devices = new List<Device>();

            //Split the output for better usage
            String deviceString = ExecuteAdbCommandWithOutput("devices -l", null);
            String[] deviceLines = deviceString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //Check whether a device is connected
            if(deviceLines.Length == 1 && deviceLines[0].Contains("List of devices attached"))
            {
                return devices;
            }

            //Now parse each line
            for(int i = 0; i < deviceLines.Length; i++)
            {
                //If it is debug line: cancel
                if (deviceLines[i].Contains("List of devices attached")) continue;

                //Split the device "42033ed44253c000       device product:cs02xx model:SM_G350 device:cs02"
                String[] parts = deviceLines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                String serialNo, model, productname, name;
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

                //Detect product
                productname = parts[2].Split(new string[] { ":" }, StringSplitOptions.None)[1];

                //Detect model
                model = parts[3].Split(new string[] { ":" }, StringSplitOptions.None)[1];

                //Detect name
                name = parts[4].Split(new string[] { ":" }, StringSplitOptions.None)[1];

                //Create, update if requested and add it to result
                Device dev = new Device(serialNo, model, productname, name, state);
                if (updateThemDirectly) dev.updateInfo();

                devices.Add(dev);
            }
            
            //Return result
            return devices;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Removes adb debug lines from output
        /// </summary>
        /// <param name="adbOutput">The output to be cleaned</param>
        /// <returns>Optimized output</returns>
        internal static String RemoveAdbDebugStuff(String adbOutput)
        {
            //Store the lines to be removed in here...
            String[] blackLines = { "adb server is out of date. killing...", "* daemon not running. starting it now on port 5037 *", "* daemon started successfully *" };


            StringBuilder output = new StringBuilder();

            //Are their multiple lines?
            if(adbOutput.Contains("\n"))
            {
                //Get the lines of output
                string[] lines = adbOutput.Split(new string[] { "\n" }, StringSplitOptions.None);

                //Check the if the lines are one of the black listed lines
                for(int i = 0; i < lines.Length; i++)
                {
                    if (Utils.Array.IsInArray(blackLines, lines[i]) == -1) output.AppendLine(lines[i]);
                }
            }
            else
            {
                //Only one line
                if (Utils.Array.IsInArray(blackLines, adbOutput) == -1) output.AppendLine(adbOutput);
            }

            //Return output
            return output.ToString();
        }
            
        #endregion
    }
}
