using System;
using System.Collections.Generic;
using AndroidLib.Utils;
using System.Text;

namespace AndroidLib.Adb
{
    public class Adb
    {
        #region Public Fields

        public static Boolean ServerIsRunning
        {
            get
            {
                return Command.IsProcessRunning("adb");
            }
        }

        #endregion

        #region Public Methods

        public static void StartServer()
        {
            ExecuteAdbCommandWithOutput("start-server", null, false);
        }

        public static void StopServer()
        {
            ExecuteAdbCommandWithOutput("kill-server", null, false);
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
            //Check whether adb is active and if not start an instance
            if (!ServerIsRunning) StartServer();

            //Insert -s parameter if needed
            String cmd = command;
            if (device != null && device.SerialNumber != "") cmd = "-s " + device.SerialNumber + "" + cmd;

            //Run process via Command class
            String output = Command.RunProcessReturnOutput(ResourceManager.adbPrefix, cmd);

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
        //public static List<Device> GetConnectedDevices(Boolean updateThemDirectly = false)
        //{

        //}

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
