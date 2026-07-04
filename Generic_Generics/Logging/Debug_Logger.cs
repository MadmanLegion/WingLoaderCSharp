using Generic_Generics.Configurations;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Generic_Generics.Logging
{
    /// <summary>
    /// Provides simple, static logging utilities to append execution details directly onto a local debugging flat file.
    /// </summary>
    public static class Debug_Logger
    {
        /// <summary>
        /// Formats and logs a tracking message, automatically capturing the originating file path and calling method name.
        /// </summary>
        /// <param name="message">The custom diagnostic message to record in the log file.</param>
        /// <param name="method">The name of the method calling the logger. Automatically injected via compiler attribute.</param>
        /// <param name="file">The full source file path of the class calling the logger. Automatically injected via compiler attribute.</param>
        [MethodImpl(MethodImplOptions.NoInlining)] // Prevent the JIT compiler from optimizing away this frame

        public static void log(string message, [CallerMemberName] string method = "", [CallerFilePath] string file = "")
        {
            if (DebugMode())
                {
                if (method.ToLower().Contains("log")) //If the calling method is a logging function, then we want to walk up the stack to get the caller of that function!.
                {
                    StackTrace stackTrace = new StackTrace();
                    StackFrame frame = stackTrace.GetFrame(2)!; //get caller of caller since log will typically be called by a Logging function
                    if (frame != null)
                    {
                        MethodBase stackMethod = frame.GetMethod()!;
                        string methodName = stackMethod.Name;
                        string methodType = stackMethod.DeclaringType!.Name;
                        method = $"{methodType}.{methodName}=>{method}";
                    }
                }

                string workingDirectory = System.IO.Directory.GetCurrentDirectory();
                string filename = string.Format("{0}-{1}", getAppName(), "debug.log");
                string logFile = System.IO.Path.Combine(workingDirectory, filename);
                string logMessage = string.Format("{0} - {1} - {2}", file, method, message);
                logToDebugFile(logFile, logMessage);
            }
        }

        /// <summary>
        /// Appends a raw structured text string to the designated physical target log file path.
        /// </summary>
        /// <param name="debugLogFile">The absolute path to the target log file.</param>
        /// <param name="message">The pre-formatted message payload string to append.</param>
        private static void logToDebugFile(string debugLogFile, string message)
        {
            using (StreamWriter writer = new StreamWriter(debugLogFile, true))
            {
                writer.WriteLine(message);
            }
        }

        /// <summary>
        /// Retrieves the friendly name identifying the current application domain to use as a file name prefix.
        /// </summary>
        /// <returns>A string representing the name of the execution context container.</returns>
        private static string getAppName()
        {
            string friendlyName = AppDomain.CurrentDomain.FriendlyName; //Name of Executable or library
            //string processName = Process.GetCurrentProcess().ProcessName; //Name from TaskBar
            //string assembly = (Assembly.GetEntryAssembly()).GetName().Name; //Name of Project
            return friendlyName;
        }

        /// <summary>
        /// Determine whether the application is in Debug mode.
        /// </summary>
        /// <returns></returns>
        public static bool DebugMode()
        {
            string? configValue = ConfigurationHandler.getConfig("appsettings:debug");
            return string.Equals(configValue, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
