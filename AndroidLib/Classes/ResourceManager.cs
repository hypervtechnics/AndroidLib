using System;
using System.IO;

namespace AndroidLib
{
    public class ResourceManager
    {
        public static String adbPrefix = "adb"; //If binaries are in an environment path just set it to "adb" otherwise it has to be fullpath with filename
        public static String tempPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
