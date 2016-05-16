namespace AndroidLib.Interaction
{
    public class BackupFileEncryptedInformation
    {
        internal string iV;
        internal string mK;
        internal string mKChecksum;
        internal string keyBytes;
        internal string saltBytes;
        internal string mKAsString;
        internal string keyFormat;

        /// <summary>
        /// The IV value of the backup file
        /// </summary>
        public string IV
        {
            get
            {
                return iV;
            }
        }

        /// <summary>
        /// The MK value of the backup file
        /// </summary>
        public string MK
        {
            get
            {
                return mK;
            }
        }

        /// <summary>
        /// The MK checksum of the backup file
        /// </summary>
        public string MKChecksum
        {
            get
            {
                return mKChecksum;
            }
        }

        /// <summary>
        /// The bytes of the key
        /// </summary>
        public string KeyBytes
        {
            get
            {
                return keyBytes;
            }
        }

        /// <summary>
        /// The bytes of the salt
        /// </summary>
        public string SaltBytes
        {
            get
            {
                return saltBytes;
            }
        }

        /// <summary>
        /// The MK as string
        /// </summary>
        public string MKAsString
        {
            get
            {
                return mKAsString;
            }
        }

        /// <summary>
        /// The format of the key, often "RAW"
        /// </summary>
        public string KeyFormat
        {
            get
            {
                return keyFormat;
            }
        }

        /// <summary>
        /// Checkes whether all values are empty
        /// </summary>
        /// <returns></returns>
        public bool ValuesAreEmpty()
        {
            return iV == "" && mK == "" && mKChecksum == "" && mKAsString == "" && saltBytes == "" && keyBytes == "" && keyFormat == "";
        }

        internal BackupFileEncryptedInformation()
        {
            this.iV = "";
            this.mK = "";
            this.mKChecksum = "";
            this.keyBytes = "";
            this.saltBytes = "";
            this.mKAsString = "";
            this.keyFormat = "";
        }
    }
}