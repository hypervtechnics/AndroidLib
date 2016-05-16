using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidLib.Utils;

namespace AndroidLib.Interaction
{
    public class BackupFile
    {
        /// <summary>
        /// Creates a BackupFile from the given path
        /// </summary>
        /// <param name="path">Path of the .ab file</param>
        /// <returns>The BackupFile object</returns>
        public static BackupFile FromFile(string path)
        {
            return new BackupFile(path);
        }

        private string mPassword;
        private string mFilePath;
        private BackupFileInfo mFileInfo;

        internal BackupFile(string path)
        {
            mPassword = "none";
            mFilePath = path;
            mFileInfo = BackupFileInfo.FromFile(path, mPassword);
        }

        /// <summary>
        /// Returns an <see cref="System.IO.FileInfo"/> for the backup file
        /// </summary>
        public FileInfo File
        {
            get
            {
                return new FileInfo(mFilePath);
            }
        }

        /// <summary>
        /// An <see cref="AndroidLib.Interaction.BackupFileInfo"/> object containing information about the backup file
        /// </summary>
        public BackupFileInfo BackupInfo
        {
            get
            {
                return mFileInfo;
            }
        }

        /// <summary>
        /// Set password of the backup file
        /// </summary>
        /// <param name="password">The password to set to</param>
        /// <returns>True if the password is correct</returns>
        public bool SetPassword(string password)
        {
            mFileInfo = BackupFileInfo.FromFile(mFilePath, password);

            return !mFileInfo.EncryptedInformation.ValuesAreEmpty();
        }
    }
}
