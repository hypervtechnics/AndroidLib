using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Results
{
    public class AdbInstallResult
    {
        private Boolean mSuccess;
        private string mOutput;

        public AdbInstallResult(Boolean success, string output)
        {
            this.mSuccess = success;
            this.mOutput = output;
        }

        /// <summary>
        /// Indicates whether the installation was successful or not
        /// </summary>
        public Boolean Success
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
