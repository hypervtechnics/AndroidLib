using System;

namespace AndroidLib.Interaction
{
    public class FileSystemObject
    {
        private string path;
        private string filename;
        private string owner;
        private string group;
        private long size;
        private FileSystemObjectPermissions permissions;
        private bool isDirectory;
        private bool isLink;
        private DateTime lastEdit;

        public FileSystemObject(string path, string filename, string owner, string group, long size, FileSystemObjectPermissions permissions, bool isDirectory, bool isLink, DateTime lastEdit)
        {
            this.path = path;
            this.filename = filename;
            this.owner = owner;
            this.group = group;
            this.size = size;
            this.permissions = permissions;
            this.isDirectory = isDirectory;
            this.isLink = isLink;
            this.lastEdit = lastEdit;
        }

        /// <summary>
        /// The full path including objectname
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// The name of the object
        /// </summary>
        public string Filename
        {
            get
            {
                return filename;
            }
        }

        /// <summary>
        /// The name of the owner
        /// </summary>
        public string Owner
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// The name of the group the object belongs to
        /// </summary>
        public string Group
        {
            get
            {
                return group;
            }
        }

        /// <summary>
        /// The size of the object in bytes
        /// </summary>
        public long Size
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        /// Represents the permissions for this object
        /// </summary>
        public FileSystemObjectPermissions Permissions
        {
            get
            {
                return permissions;
            }
        }

        /// <summary>
        /// Indicates whether the object is a directory
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return isDirectory;
            }
        }

        /// <summary>
        /// The time when the last change was made
        /// </summary>
        public DateTime LastEdit
        {
            get
            {
                return lastEdit;
            }
        }

        /// <summary>
        /// The object is a symbolic link. Can be resolved via <see cref="FileSystemManager.ResolveSymbolicLink(string)"/>
        /// </summary>
        public bool IsLink
        {
            get
            {
                return isLink;
            }
        }
    }
}