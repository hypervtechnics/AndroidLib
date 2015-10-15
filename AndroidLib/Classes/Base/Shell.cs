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
            mProcess.StartInfo.Arguments = "shell";
            mProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mProcess.StartInfo.CreateNoWindow = true;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.RedirectStandardInput = true;
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

        public void Dispose()
        {
            if(!mProcess.HasExited) mProcess.Kill();
            mProcess.Close();
        }
    }
}
}
