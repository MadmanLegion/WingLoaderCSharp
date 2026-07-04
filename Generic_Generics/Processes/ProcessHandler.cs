using System.Diagnostics;

namespace Generic_Generics.Processes
{
    internal static class ProcessHandler
    {
        /// <summary>
        /// Checks if the desired Process is running
        /// </summary>
        /// <param name="executableName">string for the executable name (not including .exe)</param>
        /// <returns>bool indicating whether the process is running</returns>
        private static bool isProcessRunning(string executableName)
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                if (process.ProcessName == executableName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
