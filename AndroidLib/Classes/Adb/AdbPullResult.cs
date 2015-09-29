using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Adb
{
    public class AdbPushPullResult
    {
        private int mTransferRate;
        private int mFileCountCopied;
        private Boolean mSuccess;
        private Boolean mSingleFile;
        private long mFileSize;
        private Dictionary<String, String> mFiles;
        private Double mSecondsNeeded;

        public AdbPushPullResult(int transferrate, int filecountcopied, Boolean success, Boolean singlefile, long filesize, Dictionary<String, String> files, Double secondsneeded)
        {
            this.mTransferRate = transferrate;
            this.mFileCountCopied = filecountcopied;
            this.mSuccess = success;
            this.mSingleFile = singlefile;
            this.mFileSize = filesize;
            this.mFiles = files;
            this.mSecondsNeeded = secondsneeded;
        }

        public int TransferRate
        {
            get
            {
                return mTransferRate;
            }
        }

        public int FileCountCopied
        {
            get
            {
                return mFileCountCopied;
            }
        }

        public Boolean Success
        {
            get
            {
                return mSuccess;
            }
        }

        public Boolean SingleFile
        {
            get
            {
                return mSingleFile;
            }
        }

        public long FileSize
        {
            get
            {
                return mFileSize;
            }
        }

        public Dictionary<String, String> Files
        {
            get
            {
                return mFiles;
            }
        }

        public Double SecondsNeeded
        {
            get
            {
                return mSecondsNeeded;
            }
        }

    }
}
