using System;

namespace AndroidLib
{
    public class ResourceManager
    {
        public static string adbPrefix = "adb"; //If binaries are in an environment path just set it to "adb" otherwise it has to be fullpath with filename
        public static string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
