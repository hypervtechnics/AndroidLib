using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Base
{
    public class Shell : IDisposable
    {
        private Device mDevice;
        private Process mProcess;

        internal Shell(Device device)
        {
            mDevice = device;

            if (!ADB.IsServerRunning) ADB.StartServer();

            mProcess = new Process();
            mProcess.StartInfo.FileName = ResourceManager.adbPrefix;
            mProcess.StartInfo.Arguments = " -s " + mDevice.SerialNumber + " shell";
            mProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mProcess.StartInfo.CreateNoWindow = true;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.RedirectStandardInput = true;
            mProcess.StartInfo.RedirectStandardOutput = true;
            mProcess.Start();
        }

        /// <summary>
        /// Indicates whether the shell instance is active
        /// </summary>
        public Boolean IsActive
        {
            get
            {
                return !mProcess.HasExited;
            }
        }

        /// <summary>
        /// Gets the in stream of shell
        /// </summary>
        public StreamWriter In
        {
            get
            {
                return mProcess.StandardInput;
            }
        }

        /// <summary>
        /// Gets the out stream of shell
        /// </summary>
        public StreamReader Out
        {
            get
            {
                return mProcess.StandardOutput;
            }
        }

        /// <summary>
        /// Executes the command given and returns it output
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <returns></returns>
        public List<string> RunCommand(string command)
        {
            //Ini output
            List<string> output = new List<string>();

            //Execute command
            mProcess.StandardInput.WriteLine(command);

            //Throw away first line
            mProcess.StandardOutput.ReadLine();

            //Then get first line of real output
            string lastLine = mProcess.StandardOutput.ReadLine().Trim();

            //And check whether its output or the output already exited
            while (!lastLine.Contains("@android:/"))
            {
                //Build line by myself because when shell exits script it doesnt write a line terminator and this will lead to an infinite loop
                //{
                //    string tempString = "";
                //    Boolean isLine = false;

                //    while(!isLine)
                //    {
                //        //Check if deadly line is there or if its the end of a line
                //        if (tempString.Contains("@android:/") || tempString.EndsWith("\r") || tempString.EndsWith("\n")) isLine = true;
                //        else tempString += mProcess.StandardOutput.Read();
                //    }

                //    lastLine = tempString.Trim();
                //}

                //Not optimal at the moment (TODO)
                lastLine = mProcess.StandardOutput.ReadLine().Trim();

                //If its okay add it to output
                if (!string.IsNullOrWhiteSpace(lastLine)) output.Add(lastLine);
            }

            return output;
        }

        /// <summary>
        /// Frees all resources needed by shell
        /// </summary>
        public void Dispose()
        {
            if(!mProcess.HasExited) mProcess.Kill();
            mProcess.Close();
        }
    }
}
