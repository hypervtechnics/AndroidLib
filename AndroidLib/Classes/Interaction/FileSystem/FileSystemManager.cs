using AndroidLib.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AndroidLib.Interaction
{
    public class FileSystemManager
    {
        private Device mDevice;
        private bool mHasRoot;

        internal FileSystemManager(Device device)
        {
            mDevice = device;
            mHasRoot = mDevice.HasRoot;
        }

        /// <summary>
        /// Returns the realpath of a symbolic link
        /// </summary>
        /// <param name="symLink">The path of the symlinked object</param>
        /// <returns>The same as given if it is no symbolic link</returns>
        public string ResolveSymbolicLink(string symLink)
        {
            if (mDevice.ConnectionStatus != DeviceState.Online)
            {
                return symLink;
            }

            Shell shell = mDevice.CommandShell;
            return shell.Exec("realpath \"" + symLink + "\"", mHasRoot).BeforeFirst("\r\r\n");
        }

        /// <summary>
        /// Gets the objects from the given path. Recommended to end the path with "/"
        /// </summary>
        /// <param name="dir">The path</param>
        /// <returns>A list ordered by name</returns>
        public InteractionResult<List<FileSystemObject>> GetObjectsFromDir(string dir)
        {
            List<FileSystemObject> result = new List<FileSystemObject>();

            //Safety check
            if(mDevice.ConnectionStatus != DeviceState.Online)
            {
                return new InteractionResult<List<FileSystemObject>>(result, false, new Exception("Device not online"));
            }

            //Execute command
            Shell shell = mDevice.CommandShell;
            string output = shell.Exec("ls -l -a \"" + dir + "\"", mHasRoot);

            //Check for errors
            if(output.Contains("opendir failed"))
            {
                return new InteractionResult<List<FileSystemObject>>(result, false, new Exception(output.After("opendir failed, ")));
            }

            //Split and prepare regex
            string[] lines = output.Split(new string[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //Regex regex = new Regex(@"^(?P<permissions>[dl\-][rwx\-]+) (?P<owner>\w+)\W+(?P<group>[\w_]+)\W*(?P<size>\d+)?\W+(?P<datetime>\d{4}-\d{2}-\d{2} \d{2}:\d{2}) (?P<name>.+)$");
            Regex regex = new Regex(@"^(?<permissions>[dl\-][rwx\-]+) (?<owner>\w+)\W+(?<group>[\w_]+)\W*(?<size>\d+)?\W+(?<datetime>\d{4}-\d{2}-\d{2} \d{2}:\d{2}) (?<name>.+)$");

            //Go thorough each line
            foreach (string line in lines)
            {
                Match match = regex.Match(line);

                if (!match.Success) continue;

                //Basic variables
                bool isDir = false;
                bool isLink = false;
                long size = 0L;
                string path = "";

                //Parse filname
                string filename = match.Groups["name"].Value;

                //Determining type
                if (match.Groups["permissions"].Value.StartsWith("l")) { filename = filename.BeforeFirst(" -> "); isLink = true; }
                else if(match.Groups["permissions"].Value.StartsWith("d")) { isDir = true; }

                //Building of path
                if (dir.EndsWith("/")) { path = dir + filename; }
                else { path = dir + "/" + filename; }
                
                //Parse of last edit date
                DateTime lastEdit = DateTime.ParseExact(match.Groups["datetime"].Value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                //Parse of owner
                string owner = match.Groups["owner"].Value;

                //Parse of group
                string group = match.Groups["group"].Value;

                //Parse of size if it is a file
                long.TryParse(match.Groups["size"].Value, out size);

                //Parse of permissions
                char[] pl = match.Groups["permissions"].Value.ToCharArray();
                FileSystemObjectPermissionGroup userGroup = new FileSystemObjectPermissionGroup(pl[1] == 'r', pl[2] == 'w', pl[3] == 'x');
                FileSystemObjectPermissionGroup ownerGroup = new FileSystemObjectPermissionGroup(pl[4] == 'r', pl[5] == 'w', pl[6] == 'x');
                FileSystemObjectPermissionGroup otherGroup = new FileSystemObjectPermissionGroup(pl[7] == 'r', pl[8] == 'w', pl[9] == 'x');
                FileSystemObjectPermissions permissions = new FileSystemObjectPermissions(userGroup, ownerGroup, otherGroup);

                //Add to results
                result.Add(new FileSystemObject(path, filename, owner, group, size, permissions, isDir, isLink, lastEdit));
            }

            return new InteractionResult<List<FileSystemObject>>(result, true, null); ;
        }

        /// <summary>
        /// Gets all files recursivly from the given directory. Recommended to end the path with "/"
        /// </summary>
        /// <param name="dir">The path</param>
        /// <returns>The list with all files</returns>
        public InteractionResult<List<FileSystemObject>> GetObjectsFromDirRecursive(string dir)
        {
            List<FileSystemObject> result = new List<FileSystemObject>();
            Queue<string> toScan = new Queue<string>();
            bool root = mHasRoot ? mDevice.HasRoot : false;

            toScan.Enqueue(dir);

            while(toScan.Count > 0)
            {
                InteractionResult<List<FileSystemObject>> res = this.GetObjectsFromDir(toScan.Dequeue());
                
                if(res.WasSuccessful)
                {
                    foreach(FileSystemObject fso in res.Result)
                    {
                        if (fso.IsDirectory) toScan.Enqueue(fso.Path);
                        else if (!fso.IsDirectory) result.Add(fso);
                    }
                }
            }

            return new InteractionResult<List<FileSystemObject>>(result, true, null);
        }

        /// <summary>
        /// Renames and/or moves a file.
        /// </summary>
        /// <param name="sourceFile">The path to the source file</param>
        /// <param name="targetFile">The target file or path. If only a path is passed the file won't be renamed while moving.</param>
        /// <returns></returns>
        public InteractionResult<string> MoveFile(string sourceFile, string targetFile)
        {
            string output = mDevice.CommandShell.Exec("mv \"" + sourceFile + "\" \"" + targetFile + "\"", mHasRoot);

            if (output.StartsWith("failed on '"))
            {
                return new InteractionResult<string>(output, false, new Exception(output.After("' - ")));
            }else
            {
                return new InteractionResult<string>(output, true, null);
            }
        }

        /// <summary>
        /// Removes the file or folder from file system
        /// </summary>
        /// <param name="path">The path of the object</param>
        /// <returns>The output</returns>
        public InteractionResult<string> RemoveObject(string path)
        {
            string output = mDevice.CommandShell.Exec("rm -r \"" + path +"\"", mHasRoot);

            if(output.StartsWith("rm failed for"))
            {
                return new InteractionResult<string>(output, false, new Exception(output.After(path + ",")));
            }else
            {
                return new InteractionResult<string>(output, true, null);
            }
        }

        /// <summary>
        /// Creates an empty file
        /// </summary>
        /// <param name="path">The path of the file to create</param>
        public void CreateEmptyFile(string path)
        {
            mDevice.CommandShell.Exec("touch \"" + path + "\"", mHasRoot);
        }

        /// <summary>
        /// Copy an object on the device
        /// </summary>
        /// <param name="sourceFile">The file to copy</param>
        /// <param name="targetFile">The target path</param>
        /// <returns>The output</returns>
        public InteractionResult<string> CopyObject(string sourceFile, string targetFile)
        {
            string output = mDevice.CommandShell.Exec("cp \"" + sourceFile + "\" \"" + targetFile + "\"", mHasRoot);

            if(output.StartsWith("cp: "))
            {
                return new InteractionResult<string>(output, false, new Exception(output.After(output.Contains(sourceFile) ? output.After(sourceFile + ": ") : output.After(targetFile + ": "))));
            }
            else
            {
                return new InteractionResult<string>(output, true, null);
            }
        }

        /// <summary>
        /// Creates a new directory. Also used to update the permissions of an existing directory
        /// </summary>
        /// <param name="path">The path to the directory</param>
        /// <param name="createParentDirectories">Create all directories that need to be created</param>
        /// <returns>The output</returns>
        public InteractionResult<string> MakeDirectory(string path, bool createParentDirectories = false)
        {
            string command = "mkdir ";
            if(createParentDirectories) { command += "-p "; }
            command += "\"" + path + "\"";

            string output = mDevice.CommandShell.Exec(command, mHasRoot);

            if(output.StartsWith("mkdir failed for"))
            {
                return new InteractionResult<string>(output, false, new Exception(output.After("/,")));
            }
            else
            {
                return new InteractionResult<string>(output, true, null);
            }
        }

        /// <summary>
        /// Changes the permissions of the given object
        /// </summary>
        /// <param name="path">The path to the object</param>
        /// <param name="newPermissions">The permissions in octal notation</param>
        /// <returns>The output</returns>
        public InteractionResult<string> ChangePermissions(string path, int newPermissions)
        {
            string output = mDevice.CommandShell.Exec("chmod " + newPermissions + " \"" + path + "\"", mHasRoot);

            if(output.StartsWith("Unable to chmod"))
            {
                return new InteractionResult<string>(output, false, new Exception(output.After(path + ": ")));
            }else
            {
                return new InteractionResult<string>(output, true, null);
            }
        }

        /// <summary>
        /// Checks if the given object exists
        /// </summary>
        /// <param name="path">The path of the object</param>
        /// <returns>True if it exists</returns>
        public InteractionResult<bool> Exists(string path)
        {
            string output = mDevice.CommandShell.Exec("if [ -e " + path + " ]; then echo 1; fi", mHasRoot);

            return new InteractionResult<bool>(output.Contains("1"), true, null);
        }

        /// <summary>
        /// Check the object. <see cref="InteractionResult{T}.Error.Message"/> == "Not Found" when it doesnt exist. Otherwise you will get the <see cref="FileSystemObject"/>
        /// </summary>
        /// <param name="path">The object to check</param>
        /// <returns>The matching <see cref="FileSystemObject"/></returns>
        public InteractionResult<FileSystemObject> GetObject(string path)
        {
            InteractionResult<List<FileSystemObject>> filesRes = this.GetObjectsFromDir(path.GetUpperPathAndroid());

            if(!filesRes.WasSuccessful)
            {
                return new InteractionResult<FileSystemObject>(null, false, filesRes.Error);
            }

            foreach(FileSystemObject fso in filesRes.Result)
            {
                if(fso.Path.Equals(path))
                {
                    return new InteractionResult<FileSystemObject>(fso, true, null);
                }
            }

            return new InteractionResult<FileSystemObject>(null, false, new Exception("Not Found"));
        }
    }
}
