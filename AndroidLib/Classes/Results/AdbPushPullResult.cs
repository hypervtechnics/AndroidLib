using System;
using System.Collections.Generic;

namespace AndroidLib.Results
{
    public class AdbPushPullResult
    {
        private int mTransferRate;
        private Boolean mSuccess;
        private Boolean mSingleFile;
        private long mFileSize;
        private Dictionary<string, string> mFiles;
        private Double mSecondsNeeded;
        private string mOutput;
        private ErrorType mError;

        public AdbPushPullResult(int transferrate, Boolean success, Boolean singlefile, long filesize, Dictionary<string, string> files, Double secondsneeded, string output, ErrorType error)
        {
            this.mTransferRate = transferrate;
            this.mSuccess = success;
            this.mSingleFile = singlefile;
            this.mFileSize = filesize;
            this.mFiles = files;
            this.mSecondsNeeded = secondsneeded;
            this.mOutput = output;
            this.mError = error;
        }

        /// <summary>
        /// The transfer rate of the process in KB/s
        /// </summary>
        public int TransferRate
        {
            get
            {
                return mTransferRate;
            }
        }

        /// <summary>
        /// Indicates whether the command was executed successfully
        /// </summary>
        public Boolean Success
        {
            get
            {
                return mSuccess;
            }
        }
        
        /// <summary>
        /// Indicates whether it was required to build a file list or not
        /// </summary>
        public Boolean SingleFile
        {
            get
            {
                return mSingleFile;
            }
        }

        /// <summary>
        /// The amount of bytes transfered
        /// </summary>
        public long FileSize
        {
            get
            {
                return mFileSize;
            }
        }

        /// <summary>
        /// If there were multiple files these are stored in here
        /// </summary>
        public Dictionary<string, string> Files
        {
            get
            {
                return mFiles;
            }
        }

        /// <summary>
        /// Time needed to transfer the file(s)
        /// </summary>
        public Double SecondsNeeded
        {
            get
            {
                return mSecondsNeeded;
            }
        }

        /// <summary>
        /// The output of Adb
        /// </summary>
        public string Output
        {
            get
            {
                return mOutput;
            }
        }
        
        /// <summary>
        /// If an error occured you will know here which one
        /// </summary>
        public ErrorType Error
        {
            get
            {
                return mError;
            }
        }
    }
}
