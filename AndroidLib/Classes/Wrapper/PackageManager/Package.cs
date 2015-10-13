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
        private String mName;
        private String mPackageName;
        private String mAssociatedFile;
        private Boolean mEnabled;

        public Package(Boolean issystemapp, String name, String packagename, String associatedfile, Boolean enabled)
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
        public String Name
        {
            get
            {
                return mName;
            }
        }

        /// <summary>
        /// Returns the package name of the app
        /// </summary>
        public String PackageName
        {
            get
            {
                return mPackageName;
            }
        }

        /// <summary>
        /// Returns the path to the apk file on the phone
        /// </summary>
        public String AssociatedFile
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
