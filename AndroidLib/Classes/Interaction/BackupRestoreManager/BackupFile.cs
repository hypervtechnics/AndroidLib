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

        /// <summary>
        /// Extracts the file to the specified tar file
        /// </summary>
        /// <param name="targetPath">The path of the tar file</param>
        /// <returns>If the process was successful</returns>
        public bool ExtractToTarFile(string targetPath)
        {
            try
            {
                Java.Update();
                string output = Java.RunJarWithOutput(ResourceManager.abePath, new string[] { "-debug", "unpack", "\"" + mFilePath + "\"", "\"" + targetPath + "\"", mPassword });

                return output.Contains("bytes written to") && System.IO.File.Exists(targetPath) && new FileInfo(targetPath).Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Packs the .ab file from the given .tar file
        /// </summary>
        /// <param name="sourceTarFile">The tar file to pack</param>
        /// <param name="targetAbFile">The target ab file</param>
        /// <param name="version">The version of the .ab file. Use <see cref="BackupFileVersion.Version2"/> for Android 4.4.3 or higher</param>
        /// <param name="password">The password used</param>
        /// <returns>True if the process was successful</returns>
        public static bool PackFromTarFile(string sourceTarFile, string targetAbFile, BackupFileVersion version = BackupFileVersion.Version1, string password = "")
        {
            try
            {
                Java.Update();
                string output = Java.RunJarWithOutput(ResourceManager.abePath, new string[] { "-debug", version == BackupFileVersion.Version1 ? "pack" : "pack-kk", "\"" + sourceTarFile + "\"", "\"" + targetAbFile + "\"", password });

                return output.Contains("bytes written to") && System.IO.File.Exists(targetAbFile) && new FileInfo(targetAbFile).Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
