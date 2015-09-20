using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AndroidLib.Utils
{
    //Taken from https://github.com/regaw-leinad/AndroidLib and changed modifiers

    public static class Command
    {
        public static void RunProcessNoReturn(string executable, string arguments, bool waitForExit = true)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;
                p.Start();
                if (waitForExit)
                {
                    p.WaitForExit();
                }
            }
        }

        public static string RunProcessReturnOutput(string executable, string arguments)
        {
            string output;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                string regular = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                if (error.Trim() == "")
                {
                    output = regular;
                }
                else
                {
                    output = error;
                }
            }
            return output;
        }

        public static string RunProcessReturnOutput(string executable, string arguments, bool forceRegular)
        {
            string output;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                string regular = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                if (error.Trim() == "" | forceRegular)
                {
                    output = regular;
                }
                else
                {
                    output = error;
                }
            }
            return output;
        }

        public static int RunProcessReturnExitCode(string executable, string arguments)
        {
            int exitCode;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;
                p.Start();
                p.WaitForExit();
                exitCode = p.ExitCode;
            }
            return exitCode;
        }

        public static bool RunProcessOutputContains(string executable, string arguments, string containsString, bool ignoreCase = false)
        {
            string output;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                string regular = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                if (error.Trim() == "")
                {
                    output = regular;
                }
                else
                {
                    output = error;
                }
            }
            if (ignoreCase)
            {
                return output.ContainsIgnoreCase(containsString);
            }
            return output.Contains(containsString);
        }

        public static void RunProcessWriteInput(string executable, string arguments, params string[] input)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.Start();
                using (StreamWriter w = p.StandardInput)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        w.WriteLine(input[i]);
                    }
                }
                p.WaitForExit();
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        public static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                Process p = processes[i];
                if (p.ProcessName.ToLower().Contains(processName.ToLower()))
                {
                    p.Kill();
                    return;
                }
            }
        }
    }
}
