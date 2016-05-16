using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AndroidLib.Wrapper
{
    public class Backup : IDisposable
    {
        private long mLastSize;
        private Device mDevice;
        private Process mProcess;
        private Thread mThread;
        private string mFilename;
        private Boolean mIsRunning;
        private int mPollingTime;

        public event EventHandler<OnBackupCompletedArgs> OnBackupCompleted;
        public event EventHandler<OnBackupProgressChangedArgs> OnBackupProgressChanged;

        public class OnBackupCompletedArgs : EventArgs
        {
            private TimeSpan time;
            private long size;
            private string output;

            /// <summary>
            /// The time needed for the backup process
            /// </summary>
            public TimeSpan Time
            {
                get
                {
                    return time;
                }
            }

            /// <summary>
            /// The size of the complete backup
            /// </summary>
            public long Size
            {
                get
                {
                    return size;
                }
            }

            /// <summary>
            /// The path to the backup file
            /// </summary>
            public string Output
            {
                get
                {
                    return output;
                }
            }

            public OnBackupCompletedArgs(TimeSpan time, long size, string output)
            {
                this.time = time;
                this.size = size;
                this.output = output;
            }
        }
        public class OnBackupProgressChangedArgs : EventArgs
        {
            private bool errorExpected;
            private bool waitingForUserInput;
            private long size;

            /// <summary>
            /// Maybe there has been an error (equal to Timeout)
            /// </summary>
            public bool ErrorExpected
            {
                get
                {
                    return errorExpected;
                }
            }

            /// <summary>
            /// Indicates whether the user has accepted the backup request on the phone
            /// </summary>
            public bool WaitingForUserInput
            {
                get
                {
                    return waitingForUserInput;
                }
            }

            /// <summary>
            /// The size of the backup until now
            /// </summary>
            public long Size
            {
                get
                {
                    return size;
                }
            }

            public OnBackupProgressChangedArgs(bool errorExpected, bool waiting, long size)
            {
                this.errorExpected = errorExpected;
                this.waitingForUserInput = waiting;
                this.size = size;
            }
        }

        /// <summary>
        /// Indicates whether the backup is currently running
        /// </summary>
        public Boolean IsRunning
        {
            get
            {
                return mIsRunning;
            }
        }

        /// <summary>
        /// The time between the check whether there have been changes
        /// </summary>
        public int PollingTime
        {
            get
            {
                return mPollingTime;
            }
        }

        /// <summary>
        /// Gets the file size got by the last check
        /// </summary>
        public long LastSize
        {
            get
            {
                return mLastSize;
            }
        }

        internal Backup(string command, Device device, string filename, int backupCheckTime)
        {
            mProcess = new Process();
            mProcess.StartInfo.FileName = ResourceManager.adbPrefix;
            mProcess.StartInfo.Arguments = "-s " + device.SerialNumber + " " + command;
            mProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mProcess.StartInfo.CreateNoWindow = true;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.RedirectStandardOutput = true;

            mLastSize = 0L;
            mDevice = device;
            mFilename = filename;
            mIsRunning = false;
            mPollingTime = backupCheckTime;

            mThread = new Thread(watchBackupProgress);
            mThread.IsBackground = true;

            mThread.Name = "Android Backup Watcher Thread";
        }

        private void watchBackupProgress()
        {
            mIsRunning = true;
            bool waitingAlreadyRaised = false;
            int lastSizeEqualTimes = 0;
            int lastSizeCountUntilError = 30000 / mPollingTime;

            while (!mProcess.HasExited)
            {
                FileInfo file = new FileInfo(mFilename);

                if (file.Exists)
                {
                    long currentSize = file.Length;
                    
                    if (mLastSize == currentSize)
                    {
                        lastSizeEqualTimes++;
                        if(lastSizeEqualTimes >= lastSizeCountUntilError)
                        {
                            if (OnBackupProgressChanged != null) OnBackupProgressChanged(this, new OnBackupProgressChangedArgs(true, waitingAlreadyRaised, mLastSize));
                        }
                    }

                    if (currentSize == 0L && !waitingAlreadyRaised)
                    {
                        waitingAlreadyRaised = true;
                        if (OnBackupProgressChanged != null) OnBackupProgressChanged(this, new OnBackupProgressChangedArgs(false, true, mLastSize));
                    }

                    if (currentSize > mLastSize)
                    {
                        lastSizeEqualTimes = 0;
                        if (OnBackupProgressChanged != null) OnBackupProgressChanged(this, new OnBackupProgressChangedArgs(false, false, mLastSize));
                    }

                    mLastSize = currentSize;
                }

                Thread.Sleep(mPollingTime);
            }

            mIsRunning = false;
            if (OnBackupCompleted != null) OnBackupCompleted(this, new OnBackupCompletedArgs(mProcess.ExitTime - mProcess.StartTime, new FileInfo(mFilename).Exists ? new FileInfo(mFilename).Length : 0L, mProcess.StandardOutput.ReadToEnd()));
        }

        /// <summary>
        /// Start the backup process
        /// </summary>
        public void Start()
        {
            mProcess.Start();
            mThread.Start();
        }

        /// <summary>
        /// Disposes this object
        /// </summary>
        public void Dispose()
        {
            mThread.Abort();
            mThread = null;
            mIsRunning = false;

            if (!mProcess.HasExited)
            {
                mProcess.Kill();
                mProcess.Close();
            }
        }
    }
}
