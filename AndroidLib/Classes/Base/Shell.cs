using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib
{
    public class Shell
    {
        private string mSerialNo;

        internal Shell(string serialNo)
        {
            mSerialNo = serialNo;
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
            string temp = ADB.ExecuteAdbCommandWithOutput(cmd, mSerialNo);

            //Remove the last line delimiter
            return temp;
        }
    }
}
