using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Adb
{
    public class AdbPullResult
    {
        private int mFileCount;
        private Dictionary<String, String> mFiles;
        private Boolean mSuccess;
        private Boolean mSingleFile;

        public AdbPullResult(int filecount, Dictionary<String, String> files, Boolean success, Boolean singlefile)
        {
            this.mFileCount = filecount;
            this.mFiles = files;
            this.mSuccess = success;
            this.mSingleFile = singlefile;
        }


        public int FileCount
        {
            get
            {
                return mFileCount;
            }
        }

        public Dictionary<String, String> Files
        {
            get
            {
                return mFiles;
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

    }

}
