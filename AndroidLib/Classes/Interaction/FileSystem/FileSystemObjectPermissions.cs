using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Interaction
{
    public class FileSystemObjectPermissions
    {
        private FileSystemObjectPermissionGroup user;
        private FileSystemObjectPermissionGroup owner;
        private FileSystemObjectPermissionGroup other;

        /// <summary>
        /// The permissions for the user
        /// </summary>
        public FileSystemObjectPermissionGroup Group
        {
            get
            {
                return user;
            }
        }

        /// <summary>
        /// The permissions for the owner
        /// </summary>
        public FileSystemObjectPermissionGroup Owner
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// The permissions for the others
        /// </summary>
        public FileSystemObjectPermissionGroup Other
        {
            get
            {
                return other;
            }
        }

        /// <summary>
        /// Returns the permissions as a number
        /// </summary>
        /// <returns>The number</returns>
        public int GetAsNumber()
        {
            return int.Parse(owner.GetAsNumber().ToString() + user.GetAsNumber().ToString() + other.GetAsNumber().ToString());
        }

        public FileSystemObjectPermissions(FileSystemObjectPermissionGroup user, FileSystemObjectPermissionGroup owner, FileSystemObjectPermissionGroup other)
        {
            this.user = user;
            this.owner = owner;
            this.other = other;
        }
    }

    public class FileSystemObjectPermissionGroup
    {
        private bool read;
        private bool write;
        private bool execute;

        /// <summary>
        /// Group is able to read
        /// </summary>
        public bool Read
        {
            get
            {
                return read;
            }
        }

        /// <summary>
        /// Group is able to write
        /// </summary>
        public bool Write
        {
            get
            {
                return write;
            }
        }

        /// <summary>
        /// Group is able to execute
        /// </summary>
        public bool Execute
        {
            get
            {
                return execute;
            }
        }

        /// <summary>
        /// Returns the permissions as a number e.g. 5
        /// </summary>
        /// <returns>The number</returns>
        public int GetAsNumber()
        {
            if(read & write & execute)
            {
                return 7;
            }
            else if(read & write & !execute)
            {
                return 6;
            }
            else if(read & !write & execute)
            {
                return 5;
            }
            else if(read & !write & !execute)
            {
                return 4;
            }
            else if(!read & write & execute)
            {
                return 3;
            }
            else if(!read & write & !execute)
            {
                return 2;
            }
            else if(!read & !write & execute)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public FileSystemObjectPermissionGroup(bool read, bool write, bool execute)
        {
            this.read = read;
            this.write = write;
            this.execute = execute;
        }
    }
}
