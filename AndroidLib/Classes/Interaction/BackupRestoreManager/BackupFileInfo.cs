using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidLib.Utils;

namespace AndroidLib.Interaction
{
    public class BackupFileInfo
    {
        /// <summary>
        /// Reads the information from the given .ab file
        /// </summary>
        /// <param name="path">The path of the .ab file</param>
        /// <param name="password">The password to use</param>
        /// <returns>The BackupFileInfo instance</returns>
        public static BackupFileInfo FromFile(string path, string password = "none")
        {
            BackupFileInfo result = new BackupFileInfo();

            Java.Update();
            string output = Java.RunJarWithOutput(ResourceManager.abePath, new string[] { "-debug", "info", "\"" + path + "\"", password });

            result.magic = output.Between("Magic: ", "\r\n");
            result.algorithm = output.Between("Algorithm: ", "\r\n");
            result.compressed = (output.Between("Compressed: ", "\r\n").Contains("1") ? true : false);
            result.version = (output.Between("Version: ", "\r\n").Contains("1") ? BackupFileVersion.Version1 : BackupFileVersion.Version2);

            BackupFileEncryptedInformation ei = new BackupFileEncryptedInformation();
            if(!output.Contains("Exception") && output.Contains("IV: "))
            {
                ei.iV = output.Between("IV: ", "\r\n");
                ei.keyBytes = output.Between("key bytes: ", "\r\n");
                ei.keyFormat = output.Between("Key format: ", "\r\n");
                ei.mK = output.Between("MK: ", "\r\n");
                ei.mKAsString = output.Between("MK as string: ", "\r\n");
                ei.mKChecksum = output.Between("MK checksum: ", "\r\n");
                ei.saltBytes = output.Between("salt bytes: ", "\r\n");
            }
            result.encrytedInformation = ei;

            return result;
        }

        public BackupFileInfo()
        {
            version = BackupFileVersion.Version1;
            magic = "ANDROID BACKUP";
            compressed = true;
            algorithm = "none";
            encrytedInformation = new BackupFileEncryptedInformation();
        }
        
        private BackupFileEncryptedInformation encrytedInformation;
        private BackupFileVersion version;
        private string magic;
        private bool compressed;
        private string algorithm;

        /// <summary>
        /// The version of the backup file
        /// </summary>
        public BackupFileVersion Version
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// If Magic is equal to "ANDROID BACKUP" everything is okay
        /// </summary>
        public string Magic
        {
            get
            {
                return magic;
            }
        }

        /// <summary>
        /// Indicats whether this backup is encrypted
        /// </summary>
        public bool Encrypted
        {
            get
            {
                return !algorithm.Equals("none");
            }
        }

        /// <summary>
        /// Indicats whether the backup is compressed
        /// </summary>
        public bool Compressed
        {
            get
            {
                return compressed;
            }
        }

        /// <summary>
        /// Indicates the algorithm used
        /// </summary>
        public string Algorithm
        {
            get
            {
                return algorithm;
            }
        }

        /// <summary>
        /// Contains information about the hashes if the file is encrypted
        /// </summary>
        public BackupFileEncryptedInformation EncryptedInformation
        {
            get
            {
                return encrytedInformation;
            }
        }
    }
}
