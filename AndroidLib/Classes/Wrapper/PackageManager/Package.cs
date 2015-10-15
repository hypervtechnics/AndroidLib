using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Wrapper
{
    public class Package
    {
        private Boolean mIsSystemApp;
        private string mName;
        private string mPackageName;
        private string mAssociatedFile;
        private Boolean mEnabled;

        public Package(Boolean issystemapp, string name, string packagename, string associatedfile, Boolean enabled)
        {
            this.mIsSystemApp = issystemapp;
            this.mName = name;
            this.mPackageName = packagename;
            this.mAssociatedFile = associatedfile;
            this.mEnabled = enabled;
        }

        /// <summary>
        /// Indicates whether the app is a system app
        /// </summary>
        public Boolean IsSystemApp
        {
            get
            {
                return mIsSystemApp;
            }
        }
        
        /// <summary>
        /// Returns the name of the app
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
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

        /// <summary>
        /// Indicates whether the app is currently enabled or not
        /// </summary>
        public Boolean Enabled
        {
            get
            {
                return mEnabled;
            }
        }
    }
}
