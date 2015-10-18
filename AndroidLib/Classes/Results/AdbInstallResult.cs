using System;

namespace AndroidLib.Results
{
    public class AdbInstallResult
    {
        private bool mSuccess;
        private string mOutput;

        public AdbInstallResult(bool success, string output)
        {
            this.mSuccess = success;
            this.mOutput = output;
        }

        /// <summary>
        /// Indicates whether the installation was successful or not
        /// </summary>
        public bool Success
        {
            get
            {
                return mSuccess;
            }
        }

        /// <summary>
        /// If Success is false then you can see the output of it here
        /// </summary>
        public string Output
        {
            get
            {
                return mOutput;
            }
        }
    }
}
