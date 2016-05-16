using Microsoft.Win32;
using System;
using System.IO;

namespace AndroidLib.Utils
{
    //Taken from https://github.com/regaw-leinad/AndroidLib and added RunJarWithOutput

    /// <summary>
    /// Contains information about the current machine's Java installation
    /// </summary>
    public static class Java
    {
        private static bool isInstalled;

        private static string installationPath;

        private static string binPath;

        private static string javaExecutable;

        private static string javacExecutable;

        /// <summary>
        /// Gets a value indicating if Java is currently installed on the local machine
        /// </summary>
        public static bool IsInstalled
        {
            get
            {
                return Java.isInstalled;
            }
        }

        /// <summary>
        /// Gets a value indicating the installation path of Java on the local machine
        /// </summary>
        public static string InstallationPath
        {
            get
            {
                return Java.installationPath;
            }
        }

        /// <summary>
        /// Gets a value indicating the path to Java's bin directory on the local machine
        /// </summary>
        public static string BinPath
        {
            get
            {
                return Java.binPath;
            }
        }

        /// <summary>
        /// Gets a value indicating the path to Java.exe on the local machine
        /// </summary>
        public static string JavaExe
        {
            get
            {
                return Java.javaExecutable;
            }
        }

        /// <summary>
        /// Gets a value indicating the path to Javac.exe on the local machine
        /// </summary>
        public static string JavacExe
        {
            get
            {
                return Java.javacExecutable;
            }
        }

        static Java()
        {
            Java.Update();
        }

        /// <summary>
        /// Updates the information stored in the <see cref="T:AndroidLib.Utils.Java" /> class
        /// </summary>
        /// <remarks>Generally called if Java installation might have changed</remarks>
        public static void Update()
        {
            Java.installationPath = Java.GetJavaInstallationPath();
            Java.isInstalled = !string.IsNullOrEmpty(Java.installationPath);
            if (Java.isInstalled)
            {
                Java.binPath = Path.Combine(Java.installationPath, "bin");
                Java.javaExecutable = Path.Combine(Java.installationPath, "bin\\java.exe");
                Java.javacExecutable = Path.Combine(Java.installationPath, "bin\\javac.exe");
            }
        }

        private static string GetJavaInstallationPath()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }
            string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            try
            {
                using (RegistryKey r = Registry.LocalMachine.OpenSubKey(javaKey))
                {
                    RegistryKey expr_28 = r;
                    using (RegistryKey i = expr_28.OpenSubKey(expr_28.GetValue("CurrentVersion").ToString()))
                    {
                        environmentPath = i.GetValue("JavaHome").ToString();
                    }
                }
            }
            catch
            {
                environmentPath = null;
            }
            return environmentPath;
        }

        /// <summary>
        /// Runs the specified Jar file with the specified arguments
        /// </summary>
        /// <param name="pathToJar">Full path the Jar file on local machine</param>
        /// <param name="arguments">Arguments to pass to the Jar at runtime</param>
        /// <returns>True if successful run, false if Java is not installed or the Jar does not exist</returns>
        public static bool RunJar(string pathToJar, params string[] arguments)
        {
            if (!Java.isInstalled)
            {
                return false;
            }
            if (!File.Exists(pathToJar))
            {
                return false;
            }
            string args = "-jar " + pathToJar;
            for (int i = 0; i < arguments.Length; i++)
            {
                args = args + " " + arguments[i];
            }
            Command.RunProcessNoReturn(Java.javaExecutable, args, true);
            return true;
        }

        /// <summary>
        /// Runs the specified Jar file with the specified arguments and gives back its arguments
        /// </summary>
        /// <param name="pathToJar">Full path the Jar file on local machine</param>
        /// <param name="arguments">Arguments to pass to the Jar at runtime</param>
        /// <returns>True if successful run, false if Java is not installed or the Jar does not exist</returns>
        public static string RunJarWithOutput(string pathToJar, params string[] arguments)
        {
            string args = "-jar " + pathToJar;
            for (int i = 0; i < arguments.Length; i++)
            {
                args = args + " " + arguments[i];
            }
            return Command.RunProcessReturnOutput(Java.javaExecutable, args);
        }
    }
}
