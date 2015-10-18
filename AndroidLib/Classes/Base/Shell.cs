using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Base
{
    public class Shell
    {
        private Device mDevice;

        internal Shell(Device device)
        {
            mDevice = device;
        }

        /// <summary>
        /// Executes the command given and returns it output
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="runAsRoot">If superuser privilegs are required</param>
        /// <returns>The output of the shell command</returns>
        public string Exec(string command, bool execAsRoot = false)
        {
            //Form command
            string cmd = "shell ";
            if (execAsRoot) cmd += "su -c ";
            cmd += command;

            //Execute and return output
            string temp = ADB.ExecuteAdbCommandWithOutput(cmd, mDevice);

            //Remove the last line delimiter
            return temp.Substring(0, temp.LastIndexOf("\r\r\n"));
        }
    }
}
