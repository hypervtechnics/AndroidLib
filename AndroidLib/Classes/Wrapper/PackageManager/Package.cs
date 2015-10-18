using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Wrapper
{
    public class Package
    {
        private bool mIsSystemApp;
        private string mPackageName;
        private string mAssociatedFile;

        public Package(bool issystemapp, string packagename, string associatedfile)
        {
            this.mIsSystemApp = issystemapp;
            this.mPackageName = packagename;
            this.mAssociatedFile = associatedfile;
        }

        /// <summary>
        /// Indicates whether the app is a system app
        /// </summary>
        public bool IsSystemApp
        {
            get
            {
                return mIsSystemApp;
            }
        }

        /// <summary>
        /// Returns the package name of the app
        /// </summary>
        public string PackageName
        {
            get
            {
                return mPackageName;
            }
        }

        /// <summary>
        /// Returns the path to the apk file on the phone
        /// </summary>
        public string AssociatedFile
        {
            get
            {
                return mAssociatedFile;
            }
        }
    }
}
