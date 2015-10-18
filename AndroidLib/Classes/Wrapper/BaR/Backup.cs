using AndroidLib.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidLib.Wrapper
{
    public class Backup : IDisposable
    {
        private long mLastSize;
        private Device mDevice;
        private Process mProcess;
        private Thread mThread;
        private string mFilename;

        public event EventHandler<OnBackupCompletedArgs> OnBackupCompleted;
        public event EventHandler<OnBackupProgressChangedArgs> OnBackupProgressChanged;

        public class OnBackupCompletedArgs : EventArgs
        {
            public TimeSpan Time;
            public long Size;
            public string Output;

            public OnBackupCompletedArgs(TimeSpan time, long size, string output)
            {
                Time = time;
                Size = size;
                Output = output;
            }
        }

        public class OnBackupProgressChangedArgs : EventArgs
        {
            public bool WaitingForUserInput;
            public long Size;

            public OnBackupProgressChangedArgs(bool waiting, long size)
            {
                WaitingForUserInput = waiting;
                Size = size;
            }
        }

        internal Backup(string command, Device device, string filename)
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

            mThread = new Thread(() =>
            {
                bool waitingAlreadyRaised = false;

                while(!mProcess.HasExited)
                {
                    FileInfo file = new FileInfo(mFilename);

                    if(file.Exists)
                    {
                        long currentSize = file.Length;

                        if(currentSize == 0L && !waitingAlreadyRaised)
                        {
                            waitingAlreadyRaised = true;
                            if (OnBackupProgressChanged != null) OnBackupProgressChanged(this, new OnBackupProgressChangedArgs(true, mLastSize));
                        }

                        if(currentSize > mLastSize)
                        {
                            if (OnBackupProgressChanged != null) OnBackupProgressChanged(this, new OnBackupProgressChangedArgs(false, mLastSize));
                        }

                        mLastSize = currentSize;
                    }

                    Thread.Sleep(3000);
                }

                if (OnBackupCompleted != null) OnBackupCompleted(this, new OnBackupCompletedArgs(mProcess.ExitTime - mProcess.StartTime, new FileInfo(mFilename).Length, mProcess.StandardOutput.ReadToEnd()));
            })
            { IsBackground = true };
        }

        public void Start()
        {
            mProcess.Start();
            mThread.Start();
        }

        public void Dispose()
        {
            mThread.Abort();
            mThread = null;

            if (!mProcess.HasExited)
            {
                mProcess.Kill();
                mProcess.Close();
            }
        }
    }
}
