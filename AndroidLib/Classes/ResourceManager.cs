using System;

namespace AndroidLib
{
    public class ResourceManager
    {
        public static string adbPrefix = "adb"; //If binaries are in an environment path just set it to "adb" otherwise it has to be fullpath with filename
        public static string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //To manage temporary file operations
        public static string abePath = "abe.jar"; //If binary is in an environment path just set it to "abe.jar" otherwise it has to be fullpath with filename
    }
}
